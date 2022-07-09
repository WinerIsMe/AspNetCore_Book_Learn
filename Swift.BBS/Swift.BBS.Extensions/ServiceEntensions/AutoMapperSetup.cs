using Microsoft.Extensions.DependencyInjection;
using Swift.BBS.Extensions.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift.BBS.Extensions.ServiceEntensions
{
    /// <summary>
    /// AutoMapper 启动服务
    /// </summary>
    public static class AutoMapperSetup
    {
        public static void AddAutoMapperSetup(this IServiceCollection services)
        {
            if(services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddAutoMapper(typeof(AutoMapperConfig));
            //需要同时在Startup 中调用AutoMapper的启动服务
        }
    }
}
