using System;
using System.Collections.Generic;
using Aska.Core.EntityStorage.Abstractions;
using Aska.Core.EntityStorage.Abstractions.Extensions;
using Aska.Core.Storage.Ef;
using Microsoft.EntityFrameworkCore;

namespace Aska.Core.EntityStorage.Ef.PostgreSql
{
    public class AutoDiscoveryPostgresqlContext : EntityStorageContext
    {
        private readonly Func<string> _connectionStringProvider;
        private readonly Func<Type[]> _entityTypesProvider;

        protected AutoDiscoveryPostgresqlContext(
            Func<string> connectionStringProvider,
            Func<Type[]> entityTypesProvider)
        {
            _connectionStringProvider = connectionStringProvider;
            _entityTypesProvider = entityTypesProvider;
        }

        public AutoDiscoveryPostgresqlContext(
            IConnectionStringProvider<AutoDiscoveryPostgresqlContext> connectionStringProvider,
            ITypeDiscoveryProvider<AutoDiscoveryPostgresqlContext> typeDiscoveryProvider)
            : this(connectionStringProvider.GetConnectionString, typeDiscoveryProvider.Discover)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            base.OnConfiguring(options);

            options.UseNpgsql(_connectionStringProvider());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entityTypes= _entityTypesProvider();
            modelBuilder.RegisterTypes(entityTypes);
        }
    }
    
    internal static class ModelBuilderExtensions
    {
        public static void RegisterTypes(this ModelBuilder builder, IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                builder.Entity(type).ToTable(type.Name)
                    .HasNoKey();//todo
            }
        }
    }
}