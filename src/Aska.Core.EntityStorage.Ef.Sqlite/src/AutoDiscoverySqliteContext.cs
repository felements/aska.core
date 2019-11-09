using System;
using System.Collections.Generic;
using Aska.Core.EntityStorage.Abstractions;
using Aska.Core.EntityStorage.Abstractions.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Aska.Core.Storage.Ef.Sqlite
{
    public class AutoDiscoverySqliteContext : EntityStorageContext
    {
        private readonly IConnectionStringProvider<AutoDiscoverySqliteContext> _connectionStringProvider;
        private readonly ITypeDiscoveryProvider<AutoDiscoverySqliteContext> _typeProvider;

        public AutoDiscoverySqliteContext(
            IConnectionStringProvider<AutoDiscoverySqliteContext> connectionStringProvider,
            ITypeDiscoveryProvider<AutoDiscoverySqliteContext> typeProvider)
        {
            _connectionStringProvider = connectionStringProvider;
            _typeProvider = typeProvider;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            base.OnConfiguring(options);

            options.UseSqlite(_connectionStringProvider.GetConnectionString());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entityTypes= _typeProvider.Discover();
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