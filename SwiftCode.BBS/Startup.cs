using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using SwiftCode.BBS.Common.Helper;
using SwiftCode.BBS.EntityFramework.EFContext;
using SwiftCode.BBS.Extensions.ServiceExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace SwiftCode.BBS.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //var connectionStrings = Configuration.GetConnectionString("mssql-db");
            //services.AddDbContext<SwiftCodeBbsContext>(o =>
            //{
            //    o.UseLazyLoadingProxies().UseSqlServer(@"Server=.;Database=SwiftCodeBbs;Trusted_Connection=True;Connection Timeout=600;MultipleActiveResultSets=true;", oo => oo.MigrationsAssembly("SwiftCode.BBS.EntityFramework"));
            //});

            var connectionStrings = Configuration.GetConnectionString("mssql-db");
            services.AddDbContext<SwiftCodeBbsContext>(o =>
                o.UseLazyLoadingProxies().UseSqlServer(connectionStrings,
                    oo => oo.MigrationsAssembly("SwiftCode.BBS.EntityFramework")));

            services.AddSingleton(new Appsettings(Configuration));
            services.AddMemoryCache();
            services.AddAutoMapperSetup();


            //services.AddSingleton(new Appsettings(Configuration));
            //var connectionStrings = Configuration.GetConnectionString("mssql - db");
            //services.AddDbContext<SwiftCodeBbsContext>(o => o.UseSqlServer(connectionStrings));



            services.AddSwaggerGen(c =>
            {
                //����С��
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                //��header����� token ת�ݵ����
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT��Ȩ�����ݽ�������ͷ�н��д��䣩ֱ�����¿������� Bearer{ token }��ע������֮����һ���ո�",
                    Name = "Authorization",//jwtĬ�ϵĲ�������
                    In = ParameterLocation.Header,//jwt Ĭ�ϴ��authorization��Ϣ��λ�ã�����ͷ��
                    Type = SecuritySchemeType.ApiKey
                });



                //c.SwaggerDoc("v1", new OpenApiInfo { Title = "SwiftCode.BBS.API", Version = "v1" });
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v0.1.0",
                    Title = "SwiftCode.BBS.API",
                    Description = "���˵���ĵ�",
                    Contact = new OpenApiContact
                    {
                        Name = "SwiftCode",
                        Email = "SwiftCode@xxx.com",
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                c.IncludeXmlComments(xmlPath,true);
            });

          
            services.AddAuthentication(x =>
            {
                //������Ǵ�����ʾ���ֵ���Ϣ
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var audienceConfig = Configuration["Audience:Audience"];
                var symmetricKeyAsBase64 = Configuration["Audience:Secret"];
                var iss = Configuration["Audience:Issuer"];
                var KeyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
                var signingKey = new SymmetricSecurityKey(KeyByteArray);
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,//���������±�
                    ValidateIssuer = true,
                    ValidIssuer = iss,//������
                    ValidateAudience = true,
                    ValidAudience = audienceConfig,//������
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,// �������ʱ�䣬����Ҫ���ǣ�����ʱ�� + ����ʱ�� �� Ĭ��7���� ����ֱ������Ϊ 0
                    RequireAudience = true,
                };
            });

            // 1����Ȩ����������ϱߵ�����ͬ�����ô����ǲ�����controller�У�д��� roles ��
            // Ȼ����ôд [Authorize(Policy = "Admin")]
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());//������ɫ
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("Admin", "System"));//��Ĺ�ϵ
                options.AddPolicy("SystemAndAdmin", policy => policy.RequireRole("Admin").RequireRole("System"));//�ҵĹ�ϵ
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SwiftCode.BBS.API v1"));
            }

          
            app.UseHttpsRedirection();

            app.UseRouting();
            //������֤
            app.UseAuthentication();
            //��Ȩ�м��
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
