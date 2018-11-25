using System.Collections.Generic;
using Autofac;
using kd.infrastructure.CommandQuery;
using kd.infrastructure.CommandQuery.Interfaces;
using kd.infrastructure.CommandQuery.Query;
using kd.infrastructure.CommandQuery.Specification;
using kd.misc;
using NLog;

namespace kd.infrastructure
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

            builder.RegisterGeneric(typeof(FulltextMatchSpecification<>))
                .As(typeof(IFulltextMatchSpecification<>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(FulltextDbQuery<,>))
                .As(typeof(IQuery<,>))
                .InstancePerDependency();

            builder.RegisterGeneric(typeof(DbQuery<,>))
                .As(typeof(IQuery<,>))
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