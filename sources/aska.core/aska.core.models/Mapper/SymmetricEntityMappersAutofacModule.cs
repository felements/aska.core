namespace aska.core.models.Mapper
{
    public class SymmetricEntityMappersAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ObjectEntityToObjectEntityMapperProfile>()
                .As<Profile>()
                .InstancePerLifetimeScope();

        }
    }
}