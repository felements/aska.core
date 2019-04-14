using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using aska.core.common;
using aska.core.infrastructure.data.CommandQuery.Interfaces;

namespace aska.core.infrastructure.data.CommandQuery.Query
{
    /// <summary>
    /// Throws NotImplemented exception on all method calls.
    /// Useful in queries that implement only one or two methods
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TSpecification"></typeparam>
    public class NopeQuery<TEntity, TSpecification> : IQuery<TEntity, TSpecification>
        where TEntity : class, IEntity
        where TSpecification : ISpecification<TEntity>
    {
        public virtual IQuery<TEntity, TSpecification> Where(Expression<Func<TEntity, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public virtual IQuery<TEntity, TSpecification> Where(TSpecification specification)
        {
            throw new NotImplementedException();
        }

        public virtual  IQuery<TEntity, TSpecification> OrderBy<TProperty>(Expression<Func<TEntity, TProperty>> expression, SortOrder sortOrder = SortOrder.Ascending)
        {
            throw new NotImplementedException();
        }

        public virtual TEntity Single()
        {
            throw new NotImplementedException();
        }

        public virtual Task<TEntity> SingleAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public virtual TEntity SingleOrDefault()
        {
            throw new NotImplementedException();
        }

        public virtual Task<TEntity> SingleOrDefaultAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public virtual TEntity FirstOrDefault()
        {
            throw new NotImplementedException();
        }

        public virtual Task<TEntity> FirstOrDefaultAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<TEntity> All()
        {
            throw new NotImplementedException();
        }

        public virtual Task<IEnumerable<TEntity>> AllAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public virtual bool Any()
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> AnyAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<TEntity> Paged(int? pageNumber, int? take)
        {
            throw new NotImplementedException();
        }

        public virtual int Count()
        {
            throw new NotImplementedException();
        }

        public virtual Task<int> CountAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}