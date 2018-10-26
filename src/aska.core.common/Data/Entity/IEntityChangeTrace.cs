using System;

namespace kd.domainmodel.Entity
{
    public interface IEntityChangeTrace
    {
        DateTime LastModifiedDateUtc { get; set; }
        string LastModifiedBy { get; set; }
    }
}