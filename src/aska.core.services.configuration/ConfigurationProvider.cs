using System;

namespace ferriswheel.services.config
{
    [Flags]
    public enum ConfigurationProvider
    {
        File = 2,
        Database = 4,
        Precompiled = 8,

        All = File | Database | Precompiled,
        NoDb = File | Precompiled,
    }
}