using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift.BBS.Common.Helper
{
    /// <summary>
    /// appsetting.json文件访问帮助类
    /// </summary>
    public class Appsettings
    {
        static IConfiguration Configuration { get; set; }
        static string contentPath { get; set; }

        public Appsettings(string contentPath)
        {
            string Path = "appsetting.json";

            //如果配置文件是根据环境变量来区分的，可以这样设置
            //Path = $"appsetting.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMWNT")}.json";

            Configuration = new ConfigurationBuilder()
                .SetBasePath(contentPath)
                //这样可以直接读取目录里的json文件，而不是bin文件夹下的，所以不用修改复制属性
                .Add(new JsonConfigurationSource { Path = Path, Optional = false, ReloadOnChange = true })
                .Build();
        }

        public Appsettings(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 封装要操作的字符
        /// </summary>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static string app(params string[] sections)
        {
            try
            {
                if (sections.Any())
                {
                    return Configuration[string.Join(":", sections)];
                }
            }
            catch (Exception) { }

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
            Configuration.Bind(string.Join(":", sections), list);
            return list;
        }
































    }
}
