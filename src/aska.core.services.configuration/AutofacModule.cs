using Autofac;
using NLog;

namespace ferriswheel.services.config
{
    public class AutofacModule : Module
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected override void Load(ContainerBuilder builder)
        {
            Logger.Debug("Register " + typeof(MergedConfigurationProvider).Name);
            builder.RegisterType<MergedConfigurationProvider>()
                .As<IConfigurationProvider>()
                .SingleInstance();
        }
    }
}