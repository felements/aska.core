using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aska.Core.EntityStorage.Abstractions;
using Aska.Core.EntityStorage.Abstractions.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aska.Core.Storage.Ef
{
    public class AutoDiscoveryDatabaseContext<TBase>: DbContext, IEntityStorageContext
    {
        private readonly string _assemblyNamePrefix;
        private readonly ITypeDiscoveryProvider _typeDiscoveryProvider;

        public AutoDiscoveryDatabaseContext(
            string assemblyNamePrefix,
            ITypeDiscoveryProvider typeDiscoveryProvider,
            DbContextOptions<AutoDiscoveryDatabaseContext<TBase>> options) : base(options)
        {
            _assemblyNamePrefix = assemblyNamePrefix;
            _typeDiscoveryProvider = typeDiscoveryProvider;
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var types = _typeDiscoveryProvider.Discover(typeof(TBase), _assemblyNamePrefix);
            modelBuilder.RegisterTypes(types);
        }


        public virtual void Initialize()
        {
        }

        public virtual IQueryable<T> Get<T>() where T : class => base.Set<T>();

        public virtual void Add<T>(T entity) where T : class => base.Add(entity);

        public virtual void Add<T>(IEnumerable<T> entity) where T : class => base.AddRange(entity);

        public virtual void Update<T>(T entity) where T : class => base.Update(entity);

        public virtual void Update<T>(IEnumerable<T> entity) where T : class => base.UpdateRange(entity);

        public virtual void Remove<T>(T entity) where T : class => base.Remove(entity);

        public virtual void Remove<T>(IEnumerable<T> entity) where T : class => base.RemoveRange(entity);

        public virtual Task<int> SaveAsync(CancellationToken cancellationToken) => base.SaveChangesAsync();
    }
    
    internal static class ModelBuilderExtensions
    {
        public static void RegisterTypes(this ModelBuilder builder, IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                builder.Entity(type).ToTable(type.Name);
            }
        }
    }
    
    // todo: ef3 by default use back fields instead of properties (if it is clear what field to use)
    // todo: ef3 has a db command interceptors implementation
    // todo: async enums

    

}