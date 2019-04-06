using System;

namespace aska.core.common
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