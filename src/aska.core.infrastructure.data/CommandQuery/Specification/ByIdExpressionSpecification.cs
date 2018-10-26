using System;
using kd.domainmodel.Entity;

namespace kd.infrastructure.CommandQuery.Specification
{
    public class ByIdExpressionSpecification<TEntity> : ExpressionSpecification<TEntity> where TEntity : class, IEntity
    {
        public ByIdExpressionSpecification(Guid id) : base(entity => entity.Id == id )
        {
        }
    }
}