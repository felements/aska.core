using System;

namespace aska.core.infrastructure.data.ef.Context
{
    public interface IDbContextEntityTypesProvider
    {
        Type[] Get();
    }
    
    public interface IDbContextEntityTypesProvider<TContext> : IDbContextEntityTypesProvider
        where TContext: class, IDbContext
    {
    }
}