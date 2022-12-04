using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftCode.BBS.Extensions.AOP
{
    public class BbsLogAOP :IInterceptor
    {
        /// <summary>
        /// 实例化IInterceptor唯一方法 
        /// </summary>
        /// <param name="invocation">包含被拦截方法的信息</param>
        public void Intercept(IInvocation invocation)
        {
            var dataIntercept = @$"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}——当前执行方法{invocation.Method.Name} - 参数为：{string.Join(",",invocation.Arguments
                .Select(x => (x ?? "").ToString().ToArray()))}\r\n";

            //注意，下边方法仅仅是针对同步的策略，如果你的service是异步的，这里获取不到,如果要异步拦截联系作者
            try
            {
                invocation.Proceed();
            }
            catch (Exception ex)
            {

                dataIntercept += $"方法执行中出现异常：{ex.Message}";
            }

            //输出到当前项目日志
            var path = Directory.GetCurrentDirectory() + @"\Log";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileName = path + $@"\InterceptLog-{DateTime.Now.ToString("yyyyMMddHHmmss")}.log";

            StreamWriter sw = File.AppendText(fileName);
            sw.WriteLine(dataIntercept);
            sw.Close();
        }
    }
}
