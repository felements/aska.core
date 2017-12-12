namespace aska.core.models.ObjectEntitySchema.Commands
{
    public class ObjectEntityCommandsModule : Module
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        protected override void Load(ContainerBuilder builder)
        {
            Logger.Debug("Register {0}", typeof(CreateObjectEntityCommand).Name);
            builder.RegisterType<CreateObjectEntityCommand>()
                .AsSelf()
                .InstancePerLifetimeScope();

            Logger.Debug("Register {0}", typeof(UpdateObjectEntityCommand).Name);
            builder.RegisterType<UpdateObjectEntityCommand>()
                .AsSelf()
                .InstancePerLifetimeScope();

            Logger.Debug("Register {0}", typeof(DeleteObjectEntityCommand).Name);
            builder.RegisterType<DeleteObjectEntityCommand>()
                .AsSelf()
                .InstancePerLifetimeScope();
            
            Logger.Debug("Register {0}", typeof(ForceDeleteObjectEntityCommand).Name);
            builder.RegisterType<ForceDeleteObjectEntityCommand>()
                .AsSelf()
                .InstancePerLifetimeScope();


        }
    }
}