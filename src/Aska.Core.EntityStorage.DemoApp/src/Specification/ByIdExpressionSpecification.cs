using Aska.Core.EntityStorage.Abstractions;

namespace Aska.Core.EntityStorage.DemoApp.Specification
{
    public class ByIdExpressionSpecification<TEntity, TKey> : ExpressionSpecification<TEntity> 
        where TEntity : class, IEntity<TKey>
    {
        public ByIdExpressionSpecification(TKey id) : base(e=>e.Id.Equals(id))
        {
        }
    }
}