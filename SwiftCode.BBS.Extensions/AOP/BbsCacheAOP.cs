using Castle.DynamicProxy;
using SwiftCode.BBS.Common.MemoryCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftCode.BBS.Extensions.AOP
{
    /// <summary>
    /// 面向切面的缓存
    /// </summary>
    public class BbsCacheAOP : IInterceptor
    {
        private ICachingProvider _cache;
        //通过注入的方式，把缓存操作接口通过构造函数注入
        public BbsCacheAOP(ICachingProvider cache)
        {
            _cache = cache;
        }

        public void Intercept(IInvocation invocation)
        {
            //获取自定义缓存键
            var cacheKey = CustomCacheKey(invocation);
            //根据key获取相应的缓存值
            var cacheValue = _cache.Get(cacheKey);
            if (cacheValue != null)
            {
                //将当前获取到的缓存值，赋值给当前执行方法
                invocation.ReturnValue = cacheValue;
                return;
            }

            //去执行当前的方法
            invocation.Proceed();
            //存入缓存
            if (!string.IsNullOrWhiteSpace(cacheKey))
            {
                _cache.Set(cacheKey, invocation.ReturnValue);
            }

        }
        private string CustomCacheKey(IInvocation invocation)
        {
            var typeName = invocation.TargetType.Name;
            var methodName = invocation.Method.Name;
            //获取参数列表，我最多需要三个即可
            var methodArguments = invocation.Arguments.Select(GetArgumentValue).Take(3).ToList();


            string key = $"{typeName}:{methodName}:";
            foreach (var param in methodArguments)
            {
                key += $"{param}:";
            }

            return key.TrimEnd(':');
        }

        private string GetArgumentValue(object obj)
        {
            // 这里只是很简单的数据类型，如果参数是表达式/类等，比较复杂的，封装的比较多，也可以自己封装。
            if (obj is int || obj is long || obj is string)
                return obj.ToString();

            if (obj is DateTime)
                return ((DateTime)obj).ToString("yyyyMMddHHmmss");

            return "";
        }
    }
}
