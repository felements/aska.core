using System;
using System.Linq;
using System.Linq.Expressions;

namespace aska.core.infrastructure.mysql.Context
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class ReadonlyEnityDbContext : DbContext, IDbContext
    {
        public ReadonlyEnityDbContext(IServiceConfiguration serviceConfiguration)
            : base(GetConnectionString(serviceConfiguration))
        {
            // AHTUNG !!!!!!!! MINEN !!!
            // Do not enable LazyLoading because of problems with mappers. 
            // It causes full entity dependency tree loading that overloads the DB.
            this.Configuration.LazyLoadingEnabled = false;

            this.AutoDetectChangesEnabled = false;
            this.Configuration.ProxyCreationEnabled = true;
            Database.SetInitializer( new NullDatabaseInitializer<ReadonlyEnityDbContext>());

            // static constructors are guaranteed to only fire once per application.
            // I do this here instead of App_Start so I can avoid including EF
            // in my MVC project (I use UnitOfWork/Repository pattern instead)
            DbConfiguration.SetConfiguration(new MySqlEFConfiguration());
        }

        private static string GetConnectionString(IServiceConfiguration serviceConfiguration)
        {
            var cs = new MySqlConnectionStringBuilder
            {
                Server = serviceConfiguration.Database.Server,
                Database = serviceConfiguration.Database.Database,
                UserID = serviceConfiguration.Database.UserName,
                Password = serviceConfiguration.Database.Password
            }.ConnectionString;

#if DEBUG
            if (string.IsNullOrWhiteSpace((serviceConfiguration.Database.Server)))
            {
                cs = "server=localhost;database=avar13;user id=ava;password=ava";
            }
#endif

            return cs;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // load all assemblies with entity classes before registering them
            AssemblyExtensions.ForceLoadAssemblies(Namespace.AssemblyNamePrefix);
            modelBuilder.RegisterDerivedTypes<IReadonlyEntity>(Namespace.AssemblyNamePrefix);
            base.OnModelCreating(modelBuilder);
        }
        

        #region IDbContext implementation

        public IDbSet<T> GetDbSet<T>() where T : class
        {
            return Set<T>();
        }

        public DbSet GetDbSet(Type entityType)
        {
            return Set(entityType);
        }

        public void Truncate<T>() where T : class
        {
            throw new NotSupportedException();
        }

        public bool AutoDetectChangesEnabled
        {
            get { return Configuration.AutoDetectChangesEnabled; }
            set { Configuration.AutoDetectChangesEnabled = value; }
        }

        public bool LazyLoadingEnabled
        {
            get { return Configuration.LazyLoadingEnabled; }
            set { Configuration.LazyLoadingEnabled = value; }
        }

        public void DetectChanges()
        {
            throw new NotSupportedException();
        }

        public IQueryable<TEntity> Include<TEntity, TProperty>(IQueryable<TEntity> query,
            params Expression<Func<TEntity, TProperty>>[] properties)
        {
            return properties.Aggregate(query, (current, property) => current.Include(property));
        }

        public void ExecuteSqlCommand(string command)
        {
            this.Database.ExecuteSqlCommand(command);
        }

        #endregion
    }
}