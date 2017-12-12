using aska.core.infrastructure.CommandQuery;
using aska.core.infrastructure.CommandQuery.Command;
using aska.core.infrastructure.CommandQuery.Interfaces;
using aska.core.infrastructure.CommandQuery.Query;
using aska.core.infrastructure.CommandQuery.Specification;
using aska.core.infrastructure.DataProtection;
using aska.core.infrastructure.Store;

namespace aska.core.infrastructure
{
    public class AutofacModule : Module
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected override void Load(ContainerBuilder builder)
        {
            // Command & Query registrations
            Logger.Debug("Register Command&Query components");
            builder.RegisterGeneric(typeof(ExpressionSpecification<>))
                .As(typeof(IExpressionSpecification<>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(DbQuery<,>))
                .As(typeof(IQuery<,>))
                .InstancePerDependency(); //IMPORTANT! Not perScope!

            builder.RegisterType<DbQueryDynamicType>()
                .As<IQuery<IEntity, ByIdExpressionSpecification<IEntity>>>()
                .InstancePerDependency();

            builder.RegisterGeneric(typeof(ByIdExpressionSpecification<>))
                .As(typeof(ISpecification<>))
                .InstancePerLifetimeScope();

            builder.RegisterType<QueryFactory>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<CommandFactory>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(TruncateCommand<>))
                //.As(typeof(ICommand<>))
                //.AsImplementedInterfaces()
                .AsSelf()
                .InstancePerLifetimeScope();

            // Legacy Repository registrations

            Logger.Debug("Register UnitOfWork");
            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();
                //.InstancePerDependency();


            Logger.Debug("Register DataEncryptor<>");
            builder.RegisterGeneric(typeof(DataEncryptor<>))
                .As(typeof(IDataEncryptor<>))
                .InstancePerLifetimeScope();
        }
    }
}