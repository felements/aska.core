using Aska.Core.EntityStorage.Abstractions;

namespace Aska.Core.EntityStorage.DemoApp.Specification;

public class ByIdExpressionSpecification<TEntity, TKey>(TKey id) : ExpressionSpecification<TEntity>(e => e.Id!.Equals(id))
    where TEntity : class, IEntity<TKey>;
