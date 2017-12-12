namespace aska.core.infrastructure.Service
{
    public interface IContainerStartup
    {
        void ApplicationContainerRegistration(ContainerBuilder builder);

        void RequestContainerRegistration(ContainerBuilder builder);
    }
}