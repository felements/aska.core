using System;

namespace aska.core.common.Data.Entity
{
    public interface IEntityChangeTrace
    {
        DateTime LastModifiedDateUtc { get; set; }
        string LastModifiedBy { get; set; }
    }
}