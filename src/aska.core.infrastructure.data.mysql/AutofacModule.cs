using Autofac;
using kd.infrastructure.mysql.Context;
using kd.infrastructure.Store;
using NLog;

namespace kd.infrastructure.mysql
{
    public class AutofacModule : Module
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected override void Load(ContainerBuilder builder)
        {
            Logger.Debug("Register MySql DB context : {0}", typeof(MysqlDbContext).Name);
            builder.RegisterType<MysqlDbContext>()
                .As<IDbContext>()
                .InstancePerLifetimeScope()
                .Named<IDbContext>("mysql");
        }
    }
}