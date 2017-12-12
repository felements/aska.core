using System;
using System.Collections.Generic;
using aska.core.networking.CacheStore;
using aska.core.networking.JsonSerializer;
using aska.core.networking.Mapper;

namespace aska.core.networking
{
    public class AutofacModule : Module
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected override void Load(ContainerBuilder builder)
        {
            var svcConfig = new ServiceConfigurationWrapper();
            Logger.Debug("**** <Application configuration output> ****");
            foreach (var cfg in svcConfig.Values)
            {
                Logger.Debug("**** {0} = [{1}]", cfg.Key, cfg.Value);
            }
            Logger.Debug("**** <Application configuration output> ****");

            Logger.Debug("Register service configuration");
            builder.RegisterInstance(svcConfig)
                .As<IServiceConfiguration>()
                .SingleInstance();

            builder.RegisterInstance(svcConfig)
                .As<IPlainServiceConfiguration>()
                .SingleInstance();

            Logger.Debug("Register authentication token handler");
            builder.RegisterType<Tokenizer>()
                .WithParameter(new TypedParameter(typeof(Action<Tokenizer.TokenizerConfigurator>),
                    (Action<Tokenizer.TokenizerConfigurator>)(tc => tc.AdditionalItems(NancyContextSecurityExtensions.GetDeviceId))))
                .As<ITokenizer>()
                .SingleInstance();

            Logger.Debug("Register JSON contract serializer");
            builder.RegisterType<CustomJsonSerializer>()
                .As<Newtonsoft.Json.JsonSerializer>()
                .SingleInstance();

            Logger.Debug("Register SiteProfileProvider");
            builder.RegisterType<SiteProfileProvider>()
                .As<ISiteProfileProvider>()
                .SingleInstance();


            Logger.Debug("Register MemoryCacheStore");
            builder.RegisterType<MemoryCacheStore>()
                .As<ICacheStore>()
                .As<ICacheStoreManagement>()
                .SingleInstance();



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

            Logger.Debug("Register encryption providers");
            builder.RegisterType<RandomKeyGenerator>()
                   .As<IKeyGenerator>()
                   .InstancePerLifetimeScope();

            builder.RegisterType<RijndaelEncryptionProvider>()
                   .As<IEncryptionProvider>()
                   .SingleInstance();

            // session store
            Logger.Debug("Register User Session Store");
            builder.RegisterType<InMemoryUserSessionStore>()
                   .As<IUserSessionStore>()
                   .SingleInstance();

            Logger.Debug("Register User Identity Resolver");
            builder.RegisterType<SessionUserIdentityResolver>()
                   .As<IUserIdentityResolver>()
                   .SingleInstance();


            


        }
    }
}