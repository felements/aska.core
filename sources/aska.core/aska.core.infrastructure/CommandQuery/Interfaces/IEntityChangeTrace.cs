using System;

namespace aska.core.infrastructure.CommandQuery.Interfaces
{
    public interface IEntityChangeTrace
    {
        DateTime LastModifiedDateUtc { get; set; }
        string LastModifiedBy { get; set; }
    }
}