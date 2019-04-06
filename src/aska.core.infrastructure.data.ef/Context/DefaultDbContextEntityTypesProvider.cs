using System;
using System.Linq;
using aska.core.common;

namespace aska.core.infrastructure.data.ef.Context
{
    public class DefaultDbContextEntityTypesProvider<TContext> : IDbContextEntityTypesProvider<TContext>
        where TContext: class, IDbContext
    {
        private readonly string _assemblyNamePrefix;

        public DefaultDbContextEntityTypesProvider(string assemblyNamePrefix)
        {
            _assemblyNamePrefix = assemblyNamePrefix;
        }
        
        public Type[] Get()
        {
            AssemblyExtensions.ForceLoadAssemblies(_assemblyNamePrefix);
            return AssemblyExtensions.GetDerivedTypes<IEntity>(_assemblyNamePrefix).ToArray();
        }

        public static DefaultDbContextEntityTypesProvider<TContext> Create(string assemblyNamePrefix)
        {
            return new DefaultDbContextEntityTypesProvider<TContext>(assemblyNamePrefix);
        }
    }
}