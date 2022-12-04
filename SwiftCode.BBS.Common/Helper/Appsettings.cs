using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SwiftCode.BBS.Common.Helper
{
    public class Appsettings
    {
        static IConfiguration _configuration { get; set; }
        static string _contentPath { get; set; }

        public Appsettings(string contentPath)
        {
            string path = "appsettings.json";

            //如果配置文件是根据环境变量区分
            //path = $"appsettings.{Environment.GetEnvironmentVariable{"ASPNETCORE_EN-VIRONMENT"}}.json";
            _configuration = new ConfigurationBuilder().SetBasePath(_contentPath)
                .Add(new JsonConfigurationSource { Path = path, Optional = false, ReloadOnChange = true })
                .Build();//直接读取目录里的json文件，而不是bin文件夹下的

        }

        public Appsettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        ///  封装操作的字符串
        /// </summary>
        /// <param name="sections">配置节点</param>
        /// <returns></returns>
        public static string app(params string[] sections)
        {
            try
            {
                if (sections.Any())
                {
                    return _configuration[string.Join(":", sections)];
                }
            }
            catch (Exception)
            {
            }
            return "";
        }

        /// <summary>
        /// 递归获取配置信息数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static List<T> app<T>(params string[] sections)
        {
            List<T> list = new List<T>();
             _configuration.Bind(string.Join(":", sections, list));
            return list;
        }

        

    }
}
