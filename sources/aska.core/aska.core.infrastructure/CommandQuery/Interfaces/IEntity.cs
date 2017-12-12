using System;
using System.Linq.Expressions;

namespace aska.core.infrastructure.CommandQuery.Interfaces
{
    public interface IEntity
    {
        //TODO: remove the Id property and use GetId expression instead. Rework UoW.Save() method to support it.
        Guid Id { get; set; }

        //Expression<Func<string>> GetIdExpression();
        Expression<Func<IEntity, bool>> CompareIdExpression();
        
    }

    public interface IRegularEntity : IEntity { }
    public interface IReadonlyEntity : IEntity { }

}