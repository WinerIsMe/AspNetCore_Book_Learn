using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Swift.BBS.Common.Helper;
using System.IO;
using System.Net.Http;

namespace Swift.BBS.Tests
{
    public class ArticleScenariosBase
    {
        //public static TestServer GetTestServer()
        //{
        //    var builder = new WebHostBuilder()
        //    .UseStartup<Startup>()
        //    .ConfigureAppConfiguration((context, config) =>
        //    {
        //        config.SetBasePath(Path.Combine(
        //    Directory.GetCurrentDirectory(),
        //    "..\\..\\..\\..\\AspNetCoreTodo"));

        //        config.AddJsonFile("appsettings.json");
        //    });

        //    var _server = new TestServer(builder);
        //    return _server;
        //}

        public static IHostBuilder GetTestHost()
        {
            return new HostBuilder()
                //替换Autofac作为DI容器
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseTestServer().UseStartup<Startup>();
                })
                .ConfigureAppConfiguration((host, builder) =>
                {
                    builder.SetBasePath(Directory.GetCurrentDirectory());
                    builder.AddJsonFile("appsetting.json", optional: true);
                    builder.AddEnvironmentVariables();
                });
        }

        public static HttpClient GetTestClientWithToken(this IHost host)
        {
            // 获取令牌
            TokenModelJwt tokenModel = new TokenModelJwt { Uid = 1, Role = "Admin" };
            var jwtStr = JwtHelper.IssueJwt(tokenModel);

            var client = host.GetTestClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtStr}");
            return client;
        }
    }
}
