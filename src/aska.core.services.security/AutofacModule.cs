using Autofac;
using kd.services.security.IdentityResolver;
using Nancy.Cryptography;
using NLog;

namespace kd.services.security
{
    public class AutofacModule : Module
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected override void Load(ContainerBuilder builder)
        {
            Logger.Debug("Register authentication token handler");
            builder.RegisterType<GoogleIdTokenIdentityResolver>()
                .As<IIdentityResolver>()
                .SingleInstance();

            Logger.Debug("Register encryption providers");
            builder.RegisterType<RandomKeyGenerator>()
                   .As<IKeyGenerator>()
                   .InstancePerLifetimeScope();

            builder.RegisterType<AesEncryptionProvider>()
            //builder.RegisterType<RijndaelEncryptionProvider>()
                   .As<IEncryptionProvider>()
                   .InstancePerLifetimeScope();

        }
    }
}