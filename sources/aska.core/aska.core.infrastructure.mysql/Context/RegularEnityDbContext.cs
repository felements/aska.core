using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace aska.core.infrastructure.mysql.Context
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class RegularEnityDbContext : DbContext, IDbContext
    {
        //private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        //public string Id { get; private set; }


        public RegularEnityDbContext(IServiceConfiguration serviceConfiguration)
            : base(GetConnectionString(serviceConfiguration))
        {
            //Id = Guid.NewGuid().ToString("D");
            //Logger.Trace("Constructing DB context w.id: {0}", Id);

            // AHTUNG !!!!!!!! MINEN !!!
            // Do not enable LazyLoading because of problems with mappers. 
            // It causes full entity dependency tree loading that overloads the DB.
            this.Configuration.LazyLoadingEnabled = false;


            this.Configuration.ProxyCreationEnabled = true;
            Database.SetInitializer<RegularEnityDbContext>(new AutomaticMigrationContextInitializer());

            // static constructors are guaranteed to only fire once per application.
            // I do this here instead of App_Start so I can avoid including EF
            // in my MVC project (I use UnitOfWork/Repository pattern instead)
            DbConfiguration.SetConfiguration(new MySqlEFConfiguration());
        }

        private static string GetConnectionString(IServiceConfiguration serviceConfiguration)
        {
            var cs = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder
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
            // I have an abstract base EntityMap class that maps Ids for my entities.
            // It is used as the base for all my class mappings
            //modelBuilder.Configurations.AddFromAssembly(typeof(EntityMap<>).Assembly);

            // identity models fix for mysql db
            // see: http://stackoverflow.com/questions/20832546/entity-framework-with-mysql-and-migrations-failing-because-max-key-length-is-76
            modelBuilder.Entity<HistoryRow>().Property(h => h.MigrationId).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<HistoryRow>().HasKey(x => x.MigrationId);
            modelBuilder.Entity<HistoryRow>().Property(h => h.ContextKey).HasMaxLength(200).IsRequired();


            // load all assemblies with entity classes before registering them
            AssemblyExtensions.ForceLoadAssemblies(Namespace.AssemblyNamePrefix);
            modelBuilder.RegisterDerivedTypes<IRegularEntity>(Namespace.AssemblyNamePrefix);
            base.OnModelCreating(modelBuilder);
        }

        public static string GetTableName(Type type, DbContext context)
        {
            var metadata = ((IObjectContextAdapter) context).ObjectContext.MetadataWorkspace;

            // Get the part of the model that contains info about the actual CLR types
            var objectItemCollection = ((ObjectItemCollection) metadata.GetItemCollection(DataSpace.OSpace));

            // Get the entity type from the model that maps to the CLR type
            var entityType = metadata
                .GetItems<EntityType>(DataSpace.OSpace)
                .Single(e => objectItemCollection.GetClrType(e) == type);

            // Get the entity set that uses this entity type
            var entitySet = metadata
                .GetItems<EntityContainer>(DataSpace.CSpace)
                .Single()
                .EntitySets
                .Single(s => s.ElementType.Name == entityType.Name);

            // Find the mapping between conceptual and storage model for this entity set
            var mapping = metadata.GetItems<EntityContainerMapping>(DataSpace.CSSpace)
                .Single()
                .EntitySetMappings
                .Single(s => s.EntitySet == entitySet);

            // Find the storage entity set (table) that the entity is mapped
            var table = mapping
                .EntityTypeMappings.Single()
                .Fragments.Single()
                .StoreEntitySet;

            // Return the table name from the storage entity set
            return (string) table.MetadataProperties["Table"].Value ?? table.Name;
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
            var tblName = GetTableName( typeof(T), this).ToLower();

            // There is an issue with sql cmd parameters formatting. Using string.format as a workaround.
            var result = this.Database.ExecuteSqlCommand( string.Format("TRUNCATE `{0}`;", tblName));
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
            ChangeTracker.DetectChanges();
        }

        public IQueryable<TEntity> Include<TEntity, TProperty>(IQueryable<TEntity> query,
            params Expression<Func<TEntity, TProperty>>[] properties)
        {
            return properties.Aggregate(query, (current, property) => current.Include(property));
        }

        public void LoadReference<TEntity, TProperty>(TEntity entity,
            Expression<Func<TEntity, TProperty>> referenceProperty, Expression<Func<TProperty, bool>> predicate = null)
            where TEntity : class
            where TProperty : class
        {
            var property = Entry(entity).Reference(referenceProperty);
            if (property.IsLoaded) return;

            if (predicate != null)
            {
                property.Query().Where(predicate).Load();
            }
            else
            {
                property.Load();
            }
        }

        public async Task LoadReferenceAsync<TEntity, TProperty>(
            TEntity entity,
            Expression<Func<TEntity, TProperty>> referenceProperty,
            Expression<Func<TProperty, bool>> predicate)
            where TEntity : class
            where TProperty : class
        {
            var property = Entry(entity).Reference(referenceProperty);
            if (!property.IsLoaded)
            {
                if (predicate != null)
                {
                    await property.Query().Where(predicate).LoadAsync();
                }
                else
                {
                    await property.LoadAsync();
                }
            }
        }

        public void LoadCollection<TEntity, TElement>(TEntity entity,
            Expression<Func<TEntity, ICollection<TElement>>> navigationProperty, Func<TElement, bool> predicate)
            where TEntity : class
            where TElement : class
        {
            var collection = Entry(entity).Collection(navigationProperty);
            if (collection.IsLoaded) return;

            if (predicate != null)
            {
                collection.Query().Where(elem => predicate(elem)).Load();
            }
            else
            {
                collection.Load();
            }
        }

        public async Task LoadCollectionAsync<TEntity, TElement>(TEntity entity,
            Expression<Func<TEntity, ICollection<TElement>>> navigationProperty, Func<TElement, bool> predicate)
            where TEntity : class
            where TElement : class
        {
            var collection = Entry(entity).Collection(navigationProperty);
            if (!collection.IsLoaded)
            {
                if (predicate != null)
                {
                    await collection.Query().Where(elem => predicate(elem)).LoadAsync();
                }
                else
                {
                    await collection.LoadAsync();
                }
            }
        }

        public void ExecuteSqlCommand(string command)
        {
            this.Database.ExecuteSqlCommand(command);
        }

        #endregion
    }
}