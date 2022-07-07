using Autofac;
using Swift.BBS.IServices;
using Swift.BBS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift.BBS.Extensions.ServiceEntensions
{
    public class AutofacModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ArticleServices>().As<IArticleServices>();
        }
    }
}
