using System.Refructure.mysql.Context;

namespace aska.core.infrastructure.data.mysql
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