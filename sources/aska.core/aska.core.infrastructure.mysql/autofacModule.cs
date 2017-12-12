using aska.core.infrastructure.mysql.Context;

namespace aska.core.infrastructure.mysql
{
    public class AutofacModule : Module
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected override void Load(ContainerBuilder builder)
        {
            Logger.Debug("Register MySql DB context : {0}", typeof(RegularEnityDbContext).Name);
            builder.RegisterType<RegularEnityDbContext>()
                .As<IDbContext>()
                //.InstancePerLifetimeScope()
                .InstancePerDependency()
                .Keyed<IDbContext>(DbContextKey.Regular);


            Logger.Debug("Register MySql DB context : {0}", typeof(ReadonlyEnityDbContext).Name);
            builder.RegisterType<ReadonlyEnityDbContext>()
                .As<IDbContext>()
                //.InstancePerLifetimeScope()
                .InstancePerDependency()
                .Keyed<IDbContext>(DbContextKey.Readonly);


            builder.RegisterType<DbContextFactory>()
                .As<IDbContextFactory<RegularEnityDbContext>>()
                .InstancePerLifetimeScope();
        }
    }
}