using aska.core.infrastructure.data.CommandQuery;
using aska.core.infrastructure.data.CommandQuery.Interfaces;
using aska.core.infrastructure.data.CommandQuery.Query;
using aska.core.infrastructure.data.CommandQuery.Specification;
using aska.core.infrastructure.data.ef.Store;
using aska.core.infrastructure.data.Store;
using Autofac;
using NLog;

namespace aska.core.infrastructure.data.ef
{
    public static class AutofacExtensions
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void RegisterEfCqrs(this ContainerBuilder builder)
        {
            // Command & Query registrations
            Logger.Debug("Register Command&Query components");
            builder.RegisterGeneric(typeof(ExpressionSpecification<>))
                .As(typeof(IExpressionSpecification<>))
                .InstancePerLifetimeScope();

            //builder.RegisterGeneric(typeof(FulltextMatchSpecification<>))
            //    .As(typeof(IFulltextMatchSpecification<>))
            //    .InstancePerLifetimeScope();

            //builder.RegisterGeneric(typeof(FulltextDbQuery<,>))
            //    .As(typeof(IQuery<,>))
            //    .InstancePerDependency();

            builder.RegisterGeneric(typeof(DbQuery<,>))
                .As(typeof(IQuery<,>))
                .WithParameter(
                    (info, context) => info.Name == "dbsetQueryCreator", 
                    (info, context) => context.Resolve<IDbContext>() )//todo: check it
                .InstancePerDependency(); //IMPORTANT! Not perScope!

            builder.RegisterGeneric(typeof(ByIdExpressionSpecification<>))
                .As(typeof(ISpecification<>))
                .InstancePerDependency();

            builder.RegisterType<QueryFactory>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<CommandFactory>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            // Legacy Repository registrations
            Logger.Debug("Register UnitOfWork");
            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerDependency();
        }
    }
}
