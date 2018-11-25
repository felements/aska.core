using System;
using System.Linq.Expressions;
using aska.core.common.Data.Entity;
using aska.core.infrastructure.data.CommandQuery.Interfaces;

namespace aska.core.infrastructure.data.CommandQuery.Specification
{
    public class FulltextMatchSpecification<TEntity> : IFulltextMatchSpecification<TEntity> where TEntity: class, IEntity
    {
        public Expression<Func<TEntity, string>>[] FieldSelectors { get; private set; }
        public string[] SearchQuery { get; private set; }

        public FulltextMatchSpecification(string[] searchQuery,  params Expression<Func<TEntity, string>>[] fieldSelectors)
        {
            SearchQuery = (searchQuery ?? new string[]{});
            FieldSelectors = fieldSelectors ?? new Expression<Func<TEntity, string>>[]{};
        }


        public bool IsSatisfiedBy(TEntity o)
        {
            throw new System.NotImplementedException();
        }

    }
}