using System;
using System.Linq;
using System.Threading.Tasks;
using kd.domainmodel.Entity;
using kd.infrastructure.mysql.Extensions;
using kd.infrastructure.Store;
using kd.misc;
using kd.misc.Constants;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace kd.infrastructure.mysql.Context
{
    public class MysqlDbContext : DbContext, IDbContext
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public string Id { get; private set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(GetConnectionString());

            // load model
            //AssemblyExtensions.ForceLoadAssemblies(Namespace.AssemblyNamePrefix);

            //var convention = new Microsoft.EntityFrameworkCore.Metadata.Conventions.ConventionSet();
            //var mb = new ModelBuilder(convention);
            //foreach (var definition in AssemblyExtensions.GetDerivedTypes<IEntity>(Namespace.AssemblyNamePrefix))
            //{
            //    mb.Entity(definition);
            //}
            //optionsBuilder.UseModel(mb.Model);

            //todo: causes NullReferenceException on creating initial migration (sdk 2.1.401)
        }

        public MysqlDbContext() : base()
        {
            Id = Guid.NewGuid().ToString("D");
            Logger.Trace("Created DB context with #{0}", Id);
        }

        private static string GetConnectionString(/*IServiceConfiguration serviceConfiguration*/)
        {
            //var cs = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder
            //{
            //    Server = serviceConfiguration.Database.Server,
            //    Database = serviceConfiguration.Database.Database,
            //    UserID = serviceConfiguration.Database.UserName,
            //    Password = serviceConfiguration.Database.Password
            //}.ConnectionString;

            //TODO: move to config
            return "server=localhost;database=kovalevskaya_design;user id=kovalevskaya_design;password=kovalevskaya_design";
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<UserPrincipal>();
            //modelBuilder.Entity<ImageEntity>();
            //modelBuilder.Entity<ThematicPageEntity>();
            //modelBuilder.Entity<AttachmentEntity>();
            //modelBuilder.Entity<AttachmentConversionTask>();

            // I have an abstract base EntityMap class that maps Ids for my entities.
            // It is used as the base for all my class mappings
            //modelBuilder.Configurations.AddFromAssembly(typeof(EntityMap<>).Assembly);

            // identity models fix for mysql db
            // see: http://stackoverflow.com/questions/20832546/entity-framework-with-mysql-and-migrations-failing-because-max-key-length-is-76
            //modelBuilder.Entity<HistoryRow>().Property(h => h.MigrationId).HasMaxLength(100).IsRequired();
            //modelBuilder.Entity<HistoryRow>().HasKey(x => x.MigrationId);
            //modelBuilder.Entity<HistoryRow>().Property(h => h.ContextKey).HasMaxLength(200).IsRequired();

            // load all assemblies with entity classes before registering them
            AssemblyExtensions.ForceLoadAssemblies(Namespace.AssemblyNamePrefix);
            modelBuilder.RegisterDerivedTypes<IEntity>(Namespace.AssemblyNamePrefix);

            base.OnModelCreating(modelBuilder);
        }

        #region IDbContext implementation

        
        public DbSet<T> GetDbSet<T>() where T : class
        {
            return Set<T>();
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public bool AutoDetectChangesEnabled
        {
            get => this.ChangeTracker.AutoDetectChangesEnabled;
            set => this.ChangeTracker.AutoDetectChangesEnabled = value;
        }
        
        public void DetectChanges()
        {
            ChangeTracker.DetectChanges();
        }

        //public IQueryable<TEntity> Include<TEntity, TProperty>(
        //    IQueryable<TEntity> query,
        //    params Expression<Func<TEntity, TProperty>>[] properties)
        //{
        //    return properties.Aggregate(query, (current, property) => current.Include(property));
        //}

        public string GetTableName<T>() where T : class, IEntity
        {
            return GetTableName(typeof(T), this);
        }

        public void ExecuteRawSqlCommand(string command)
        {
            if (string.IsNullOrWhiteSpace(command)) throw new ArgumentOutOfRangeException(nameof(command));

            Logger.Warn("Executing raw SQL command: " + command);
            this.Database.ExecuteSqlCommand(command);
        }

        public IQueryable<T> ExecuteRawSqlQuery<T>(string query, params object[] parameters) where T : class
        {
            if (string.IsNullOrWhiteSpace(query)) throw new ArgumentOutOfRangeException(nameof(query));

            Logger.Warn("Executing raw SQL query: " + query);

            return this.Set<T>().FromSql(query, parameters);
        }

        public string[] Migrate()
        {
            var pending = Database.GetPendingMigrations();
            Database.Migrate();
            return pending.ToArray();
        }

        public void TruncateTable<T>() where T : class, IEntity
        {
            var tblName = GetTableName(typeof(T), this).ToLower();

            ////todo:  There is an issue with sql cmd parameters formatting. Using string.format as a workaround.
            var result = this.Database.ExecuteSqlCommand(string.Format("TRUNCATE `{0}`;", tblName));
        }

        /// <summary>
        /// returns the related table name, that has been generated by EF for requesing entity type  
        /// </summary>
        /// <param name="type">Entity type</param>
        /// <param name="context">DB Context</param>
        /// <returns></returns>
        private static string GetTableName(Type type, DbContext context)
        {
            var mapping = context.Model.FindEntityType(type).Relational();
            var schema = mapping.Schema;
            var tableName = mapping.TableName;
            return tableName;
        }
        #endregion
    }

}