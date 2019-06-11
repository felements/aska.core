using System;

namespace aska.core.infrastructure.data.ef.Context
{
    public interface IDbContextMetadata
    {
        Type[] GetEntityTypes();
    }
}