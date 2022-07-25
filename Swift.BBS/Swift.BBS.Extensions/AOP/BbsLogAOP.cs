﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Swift.BBS.Extensions.AOP
{
    public class BbsLogAOP : IInterceptor
    {

        /// <summary>
        /// 实例化IInterceptor唯一方法 
        /// </summary>
        /// <param name="invocation">包含被拦截方法的信息</param>
        public void Intercept(IInvocation invocation)
        {
            //记录被拦截方法信息的日志信息
            var dataIntercept = $"{DateTime.Now.ToString("yyyyMMddHHmmss")} " +
                                $"当前执行方法：{invocation.Method.Name} " +
                                $"参数是： {string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray())} \r\n";

            //注意，下边方法仅仅是针对同步的策略，如果你的service是异步的，这里获取不到,如果要异步拦截联系作者
            try
            {
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                dataIntercept += ($"方法执行中出现异常：{ex.Message}");
            }

            // 异步获取异常，先执行
            if (IsAsyncMethod(invocation.Method))
            {
                var type = invocation.Method.ReturnType;
                var resultProperty = type.GetProperty("Result");
                dataIntercept += ($"【执行完成结果】：{JsonConvert.SerializeObject(resultProperty.GetValue(invocation.ReturnValue))}");
            }
            else
            {// 同步1

                dataIntercept += ($"【执行完成结果】：{invocation.ReturnValue}");
            }

            #region 输出到当前项目日志
            var path = Directory.GetCurrentDirectory() + @"\Log";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileName = path + $@"\InterceptLog-{DateTime.Now.ToString("yyyyMMdd")}.log";

            StreamWriter sw = File.AppendText(fileName);
            sw.WriteLine(dataIntercept);
            sw.Close();
            #endregion


        }
        private static bool IsAsyncMethod(MethodInfo method)
        {
            return (
                method.ReturnType == typeof(Task) ||
                (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                );
        }

    }
}
