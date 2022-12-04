using Autofac;
using SwiftCode.BBS.IService.BASE;
using SwiftCode.BBS.Repositories.BASE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwiftCode.BBS.Service.BASE;
using System.Reflection;
using SwiftCode.BBS.Common.Helper;
using SwiftCode.BBS.Extensions.AOP;

namespace SwiftCode.BBS.Extensions.ServiceExtensions
{
    public class AutoFacModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var cacheType = new List<Type>();

            if (Appsettings.app(new string[] { "AppSettings", "MemoryCachingAOP", "Enabled" }).ObjToBool())
            {
                builder.RegisterType<BbsCacheAOP>();
                cacheType.Add(typeof(BbsCacheAOP));
            }
            if (Appsettings.app(new string[] { "AppSettings", "LogAOP", "Enabled" }).ObjToBool())
            {
                builder.RegisterType<BbsLogAOP>();
                cacheType.Add(typeof(BbsLogAOP));
            }

            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).InstancePerDependency();

            builder.RegisterGeneric(typeof(BaseServices<>)).As(typeof(IBaseServices<>)).InstancePerDependency();

            var assemblysServices = Assembly.Load("SwiftCode.BBS.Service");//这个注入实现类，不是接口层 ，不是IService

            builder.RegisterAssemblyTypes(assemblysServices).AsImplementedInterfaces();//指定已扫描程序集中的类型

            var assemblysRepository = Assembly.Load("SwiftCode.BBS.Repositories");//模式是load(解决方案名)

            builder.RegisterAssemblyTypes(assemblysRepository).AsImplementedInterfaces();

        }
    }
}