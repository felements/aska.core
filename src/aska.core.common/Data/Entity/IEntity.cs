using System;

namespace aska.core.common.Data.Entity
{
    public interface IEntity
    {
        Guid Id { get; set; }

        //TODO: remove the Id property and use GetId expression instead. Rework UoW.Save() method to support it.
    }
}