namespace aska.core.infrastructure.mysql.Context
{
    public class DbContextFactory : IDbContextFactory<RegularEnityDbContext>
    {
        private readonly IServiceConfiguration _serviceConfiguration;

        public DbContextFactory()
        {
            _serviceConfiguration = new ServiceConfigurationWrapper();
        }

        public DbContextFactory(IServiceConfiguration serviceConfiguration)
        {
            _serviceConfiguration = serviceConfiguration;
        }

        public RegularEnityDbContext Create()
        {
            return new RegularEnityDbContext(_serviceConfiguration);
        }
    }
}