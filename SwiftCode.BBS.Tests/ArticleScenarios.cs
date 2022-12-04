using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SwiftCode.BBS.Tests
{
    public class ArticleScenarios
    {
        [Fact]
        public async Task Get_get_articles_by_page_and_response_ok_status_code()
        {
            //获取服务
            using var server = await ArticleScenariosBase.GetTestHost().StartAsync();

            //Aotion 发起接口请求
            var response = await server.GetTestClient()
                .GetAsync("/api/article/Getlist?page1&pageSize=20");
            //Assert 确保接口状态码是200
            response.EnsureSuccessStatusCode();

            // Assert 接口状态是 401 ,添加接口权限 使用
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
