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
                //开启小锁
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                //在header中添加 token 转递到后端
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT授权（数据将在请求头中进行传输）直接在下框中输入 Bearer{ token }（注意两者之间是一个空格）",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt 默认存放authorization信息的位置（请求头）
                    Type = SecuritySchemeType.ApiKey
                });



                //c.SwaggerDoc("v1", new OpenApiInfo { Title = "SwiftCode.BBS.API", Version = "v1" });
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v0.1.0",
                    Title = "SwiftCode.BBS.API",
                    Description = "框架说明文档",
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
                //这个就是错误提示出现的信息
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
                    IssuerSigningKey = signingKey,//参数配置下边
                    ValidateIssuer = true,
                    ValidIssuer = iss,//发行人
                    ValidateAudience = true,
                    ValidAudience = audienceConfig,//订阅人
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,// 缓冲过期时间，这里要考虑，过期时间 + 缓冲时间 ， 默认7分钟 可以直接设置为 0
                    RequireAudience = true,
                };
            });

            // 1【授权】、这个和上边的异曲同工，好处就是不用在controller中，写多个 roles 。
            // 然后这么写 [Authorize(Policy = "Admin")]
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());//单独角色
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("Admin", "System"));//或的关系
                options.AddPolicy("SystemAndAdmin", policy => policy.RequireRole("Admin").RequireRole("System"));//且的关系
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
            //开启认证
            app.UseAuthentication();
            //授权中间件
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
