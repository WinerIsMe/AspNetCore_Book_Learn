using Castle.DynamicProxy;
using Newtonsoft.Json;
using Swift.BBS.Common.MemoryCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Swift.BBS.Extensions.AOP
{
    /// <summary>
    /// 面向切面的缓存
    /// </summary>
    public class BbsCacheAOP : IInterceptor
    {
        //注入缓存操作接口
        private readonly ICachingProvider cache;
        public BbsCacheAOP(ICachingProvider cache)
        {
            this.cache = cache;
        }

        public void Intercept(IInvocation invocation)
        {
            //获取自定义缓存键
            var cacheKey = CustomCacheKey(invocation);
            //根据 key 获取相应的缓存值
            var cacheValue = cache.Get(cacheKey);
            if (cacheValue != null)
            {
                //将当前获取到的缓存之，赋值给当前执行的方法
                invocation.ReturnValue = cacheValue;
                return;
            }
            //去执行当前方法
            invocation.Proceed();
            //获取执行结果
            var returnValue = "";
            if (IsAsyncMethod(invocation.Method))
            {
                var type = invocation.Method.ReturnType;
                var resultProperty = type.GetProperty("Result");
                returnValue = JsonConvert.SerializeObject(resultProperty.GetValue(invocation.ReturnValue));
            }
            else
            {
                returnValue = invocation.ReturnValue.ToString();
            }
            //存入缓存
            if (!string.IsNullOrWhiteSpace(cacheKey))
            {
                cache.Set(cacheKey, returnValue);
            }
        }
        private static bool IsAsyncMethod(MethodInfo method)
        {
            return (
                method.ReturnType == typeof(Task) ||
                (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                );
        }

        private string CustomCacheKey(IInvocation invocation)
        {
            var typeName = invocation.TargetType.Name;
            var methodName = invocation.Method.Name;
            var methodArguments = invocation.Arguments.Select(GetArgumentValue).Take(3).ToList();//获取参数列表，最多需要三个即可

            string key = $"{typeName}:{methodName}:";
            foreach (var param in methodArguments)
            {
                key += $"{param}:";
            }

            return key.TrimEnd(':');
        }
        /// <summary>
        /// object 转 string
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private string GetArgumentValue(object arg)
        {
            if (arg is int || arg is long || arg is string)
                return arg.ToString();
            if (arg is DateTime)
                return ((DateTime)arg).ToString("yyyyMMddHHmmss");
            return "";
        }
    }
}
