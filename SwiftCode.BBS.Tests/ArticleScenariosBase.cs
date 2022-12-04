
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SwiftCode.BBS.API;
using SwiftCode.BBS.Common.Helper;
using SwiftCode.BBS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SwiftCode.BBS.Tests
{
    public static class ArticleScenariosBase
    {
        // 这种方式也是可行的，但是使用了Autofac 这种方式不能把Autofac 容器注入到TestServer里 所以采用第二种方式，通过自定义HostBulider 来创建Server
        //public static TestServer GetTestServer()
        //{
        //var builder = new WebHostBuilder()
        //.UseStartup<Startup>()
        //.ConfigureAppConfiguration((context, config) =>
        //{
        //config.SetBasePath(Path.Combine(
        //Directory.GetCurrentDirectory(),
        //"..\\..\\..\\..\\AspNetCoreTodo"));

        //config.AddJsonFile("appsettings.json");
        //});

        //var _server = new TestServer(builder);
        //return _server;
        //}

        public static IHostBuilder GetTestHost()
        {
            return new HostBuilder()
                //替换 Autofac 作为 DI
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                webBuilder
                .UseTestServer()
                .UseStartup<Startup>())
                .ConfigureAppConfiguration((host, builder) =>
                {
                    builder.SetBasePath(Directory.GetCurrentDirectory());
                    builder.AddJsonFile("appsettings.json", optional: true);
                    builder.AddEnvironmentVariables();
                });

        }

        public static HttpClient GetTestClientWithToken(this IHost host)
        {
            //获取令牌
            TokenModelJwt token = new TokenModelJwt { Uid = 1,Role = "Admin"};

            var jwtStr = JwtHelper.IssueJwt(token);

            var Client = host.GetTestClient();

            Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtStr}");
            
            return Client;
        }

    }
}
