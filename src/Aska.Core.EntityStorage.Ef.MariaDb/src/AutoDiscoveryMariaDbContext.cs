using System;
using System.Collections.Generic;
using Aska.Core.EntityStorage.Abstractions;
using Aska.Core.EntityStorage.Abstractions.Extensions;
using Aska.Core.Storage.Ef;
using Microsoft.EntityFrameworkCore;

namespace Aska.Core.EntityStorage.Ef.MariaDb
{
    public class AutoDiscoveryMariaDbContext : EntityStorageContext
    {
        private readonly IConnectionStringProvider<AutoDiscoveryMariaDbContext> _connectionStringProvider;
        private readonly ITypeDiscoveryProvider<AutoDiscoveryMariaDbContext> _typeDiscoveryProvider;

        public AutoDiscoveryMariaDbContext(
            IConnectionStringProvider<AutoDiscoveryMariaDbContext> connectionStringProvider,
            ITypeDiscoveryProvider<AutoDiscoveryMariaDbContext> typeDiscoveryProvider)
        {
            _connectionStringProvider = connectionStringProvider;
            _typeDiscoveryProvider = typeDiscoveryProvider;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            base.OnConfiguring(options);

            options.UseMySql(_connectionStringProvider.GetConnectionString());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entityTypes= _typeDiscoveryProvider.Discover();
            modelBuilder.RegisterTypes(entityTypes);
        }
    }
    
    internal static class ModelBuilderExtensions
    {
        public static void RegisterTypes(this ModelBuilder builder, IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                builder.Entity(type).ToTable(type.Name).HasNoKey();
            }
        }
    }
}