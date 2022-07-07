using Autofac;
using Swift.BBS.IServices;
using Swift.BBS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Swift.BBS.Extensions.ServiceEntensions
{
    public class AutofacModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ArticleServices>().As<IArticleServices>();

            //进行注入的是实现类所在层，不是接口层
            var assemblysServices = Assembly.Load("Swift.BBS.Services");
            //在 Load 方法中，指定要扫描的程序集类库名称，这样系统会自动吧该程序集中所有的接口和实现类注册到服务中
            builder.RegisterAssemblyTypes(assemblysServices).AsImplementedInterfaces();
            
            var assemblyRespository = Assembly.Load("Swift.BBS.Resposotories");
            builder.RegisterAssemblyTypes(assemblyRespository).AsImplementedInterfaces();
        }
    }
}
