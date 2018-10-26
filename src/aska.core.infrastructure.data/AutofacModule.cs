using System.Collections.Generic;
using Autofac;
using AutoMapper;
using kd.infrastructure.CommandQuery;
using kd.infrastructure.CommandQuery.Interfaces;
using kd.infrastructure.CommandQuery.Query;
using kd.infrastructure.CommandQuery.Specification;
using kd.infrastructure.Mapper;
using kd.infrastructure.ShellCommandExecutionProvider;
using kd.infrastructure.Store;
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


            #region AutoMapper
            //var mapperProfiles = AssemblyExtensions.GetDerivedTypes<Profile>(Namespace.AssemblyNamePrefix).ToArray();
            //Logger.Debug("Register Automapper profiles: \n\r - " + string.Join("\n\r - ", mapperProfiles.Select(x=>x.Name)) );

            //foreach (Type mapperProfileType in mapperProfiles)
            //{
            //    builder.RegisterType(mapperProfileType)
            //        .As<Profile>()
            //        .PreserveExistingDefaults();
            //}
            // TODO: preserve registering already added Profiles (issue with fs/web attachment mappers)
            
            Logger.Debug("Register AutoMapper instance");
            builder.Register(c => MapperFactory.Create(c.Resolve<IEnumerable<Profile>>()))
                .As<IMapper>()
                .SingleInstance();
            #endregion
            
            #region Shell cmd execution

            var env = ApplicationExtensions.GetEnvironment();
            switch (env.OsType)
            {
                case RuntimeOsType.Linux:
                case RuntimeOsType.Osx: // todo: ??
                    Logger.Debug("Register MONO shell command execution provider");
                    builder.RegisterType<MonoExecutionProvider>()
                           .As<IShellCommandExecutionProvider>()
                           .InstancePerLifetimeScope();
                    break;
                case RuntimeOsType.Windows:
                    Logger.Debug("Register WINDOWS shell command execution provider");
                    builder.RegisterType<WindowsExecutionProvider>()
                           .As<IShellCommandExecutionProvider>()
                           .InstancePerLifetimeScope();
                    break;
            }

            #endregion

        }
    }
}