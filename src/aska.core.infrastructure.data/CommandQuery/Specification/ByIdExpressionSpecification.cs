using System;
using aska.core.common;

namespace aska.core.infrastructure.data.CommandQuery.Specification
{
    public class ByIdExpressionSpecification<TEntity> : ExpressionSpecification<TEntity> where TEntity : class, IEntity
    {
        public ByIdExpressionSpecification(Guid id) : base(entity => entity.Id == id )
        {
        }
    }
}