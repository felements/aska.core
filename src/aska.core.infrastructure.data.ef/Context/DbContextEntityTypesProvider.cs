using System;
using System.Linq;
using aska.core.common;
using aska.core.infrastructure.data.Model;

namespace aska.core.infrastructure.data.ef.Context
{
    public class DbContextEntityTypesProvider<TContext> : IDbContextEntityTypesProvider<TContext>
        where TContext: class, IDbContext
    {
        private readonly string _assemblyNamePrefix;

        public DbContextEntityTypesProvider(string assemblyNamePrefix)
        {
            _assemblyNamePrefix = assemblyNamePrefix;
        }
        
        public Type[] Get()
        {
            AssemblyExtensions.ForceLoadAssemblies(_assemblyNamePrefix);
            return AssemblyExtensions.GetDerivedTypes<IEntity>(_assemblyNamePrefix).ToArray();
        }
    }
}