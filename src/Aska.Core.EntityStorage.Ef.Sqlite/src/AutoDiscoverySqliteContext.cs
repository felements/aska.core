using System;
using System.Collections.Generic;
using Aska.Core.EntityStorage.Abstractions;
using Aska.Core.EntityStorage.Abstractions.Extensions;
using Aska.Core.EntityStorage.Ef;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Aska.Core.Storage.Ef.Sqlite
{
    public class AutoDiscoverySqliteContext : EntityStorageContext
    {
        private readonly Func<string> _connectionStringProvider;
        private readonly Func<Type[]> _entityTypeProvider;

        protected AutoDiscoverySqliteContext(
            Func<string> connectionStringProvider,
            Func<Type[]> entityTypeProvider)
        {
            _connectionStringProvider = connectionStringProvider;
            _entityTypeProvider = entityTypeProvider;
        }

        public AutoDiscoverySqliteContext(
            IConnectionStringProvider connectionStringProvider,
            ITypeDiscoveryProvider typeProvider)
            : this(connectionStringProvider.GetConnectionString, typeProvider.Discover)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            base.OnConfiguring(options);

            options.UseSqlite(_connectionStringProvider());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entityTypes = _entityTypeProvider();
            modelBuilder.RegisterTypes(entityTypes);
        }
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
}