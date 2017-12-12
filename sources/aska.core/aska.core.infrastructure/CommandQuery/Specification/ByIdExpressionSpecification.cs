using System;
using System.Linq;
using aska.core.infrastructure.CommandQuery.Interfaces;

namespace aska.core.infrastructure.CommandQuery.Specification
{
    public class ByIdExpressionSpecification<TEntity> : ExpressionSpecification<TEntity> where TEntity : class, IEntity
    {
        public ByIdExpressionSpecification(Guid id) : base(entity => entity.Id == id )
        {
        }

        public ByIdExpressionSpecification(Guid[] ids) : base(entity => ids.Contains(entity.Id))
        {
        }
    }
}