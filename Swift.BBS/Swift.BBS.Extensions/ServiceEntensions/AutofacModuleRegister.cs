using Autofac;
using Swift.BBS.Extensions.AOP;
using Swift.BBS.IRepositories.BASE;
using Swift.BBS.IServices.BASE;
using Swift.BBS.Repositories.BASE;
using Swift.BBS.Services.BASE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Swift.BBS.Common.Helper;
using Autofac.Extras.DynamicProxy;

namespace Swift.BBS.Extensions.ServiceEntensions
{
    public class AutofacModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //注入AOP拦截器
            builder.RegisterType<BbsLogAOP>();

            //基接口和基类注入
            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).InstancePerDependency();
            builder.RegisterGeneric(typeof(BaseServices<>)).As(typeof(IBaseServices<>)).InstancePerDependency();

            //服务层所在dll注入
            var assemblysServices = Assembly.Load("Swift.BBS.Services");//需要的是接口实现类所在层！！
            builder.RegisterAssemblyTypes(assemblysServices)
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()//对目标类型启用接口拦截
                .InterceptedBy(typeof(BbsLogAOP));//指定拦截器
            //仓储层所在dll注入
            var assemblyRespository = Assembly.Load("Swift.BBS.Repositories");
            builder.RegisterAssemblyTypes(assemblyRespository).AsImplementedInterfaces();
            
            //针对非接口及及实现类注入
            var entityFramework = Assembly.Load("Swift.BBS.EntityFramework");
            //非接口实现类时，不能使用 AsImplementedInterfaces() 方法
            builder.RegisterAssemblyTypes(entityFramework);
        }
    }
}
