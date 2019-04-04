using System;

namespace aska.core.infrastructure.data.Model
{
    public interface IEntity
    {
        Guid Id {get;}

        string GetId();
    }

    public interface IEntityFakeDeleted{
        bool IsDeleted {get;set;}
    }
}