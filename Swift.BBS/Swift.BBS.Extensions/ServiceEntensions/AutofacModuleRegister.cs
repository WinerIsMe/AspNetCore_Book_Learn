using Autofac;
using Swift.BBS.IRepositories.BASE;
using Swift.BBS.IServices.BASE;
using Swift.BBS.Repositories.BASE;
using Swift.BBS.Services.BASE;
using System;
using System.IO;
using System.Reflection;

namespace Swift.BBS.Extensions.ServiceEntensions
{
    public class AutofacModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            #region 手动对单个实例对象进行注入

            //builder.RegisterType<ArticleServices>().As<IArticleServices>();

            #endregion

            #region 指定实现类所在的dll进行注入

            //进行注入的是实现类所在层，不是接口层
            //var assemblysServices = Assembly.Load("Swift.BBS.Services");
            //在 Load 方法中，指定要扫描的程序集类库名称，这样系统会自动吧该程序集中所有的接口和实现类注册到服务中
            //builder.RegisterAssemblyTypes(assemblysServices).AsImplementedInterfaces();

            //var assemblyRespository = Assembly.Load("Swift.BBS.Repositories");
            //builder.RegisterAssemblyTypes(assemblyRespository).AsImplementedInterfaces();

            //var assemblyEntityFramework = Assembly.Load("Swift.BBS.EntityFramework");
            //builder.RegisterAssemblyTypes(assemblyEntityFramework).AsImplementedInterfaces();

            #endregion

            #region 指定实现类dll所在的目录位置进行注入

            //var basePath = AppContext.BaseDirectory;
            //var servicesDllFile = Path.Combine(basePath, "Swift.BBS.Services.dll");
            //var respositoryDllFile = Path.Combine(basePath, "Swift.BBS.Repositories.dll");
            //var entityFrameworkDllFile = Path.Combine(basePath, "Swift.BBS.EntityFramework.dll");
            //if (!(File.Exists(servicesDllFile) && File.Exists(respositoryDllFile) && File.Exists(entityFrameworkDllFile)))
            //{
            //    var msg = "Repositories.dll 和 Services.dll 和 EntityFramework.dll 丢失";
            //    throw new Exception(msg);
            //}
            //var assemblysServices = Assembly.LoadFrom(servicesDllFile);
            //builder.RegisterAssemblyTypes(assemblysServices).AsImplementedInterfaces();

            //var assemblyRespository = Assembly.LoadFrom(respositoryDllFile);
            //builder.RegisterAssemblyTypes(assemblyRespository).AsImplementedInterfaces();

            //var assemblyEntityFramework = Assembly.LoadFrom(entityFrameworkDllFile);
            //非接口实现类时，不能使用 AsImplementedInterfaces() 方法
            //builder.RegisterAssemblyTypes(assemblyEntityFramework);

            #endregion

            #region 注入泛型仓储和服务

            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).InstancePerDependency();
            builder.RegisterGeneric(typeof(BaseServices<>)).As(typeof(IBaseServices<>)).InstancePerDependency();

            var assemblysServices = Assembly.Load("Swift.BBS.Services");
            builder.RegisterAssemblyTypes(assemblysServices).AsImplementedInterfaces();

            var assemblyRespository = Assembly.Load("Swift.BBS.Repositories");
            builder.RegisterAssemblyTypes(assemblyRespository).AsImplementedInterfaces();

            var entityFramework = Assembly.Load("Swift.BBS.EntityFramework");
            //非接口实现类时，不能使用 AsImplementedInterfaces() 方法
            builder.RegisterAssemblyTypes(entityFramework);

            #endregion
        }
    }
}
