﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Expressions;
using kd.domainmodel.Entity;
using kd.infrastructure.CommandQuery.Query;

namespace kd.infrastructure.CommandQuery.Interfaces
{
    public interface IQuery<TEntity, in TSpecification>
        where TEntity : class, IEntity
        where TSpecification : ISpecification<TEntity>
    {
        IQuery<TEntity, TSpecification> Where(Expression<Func<TEntity, bool>> expression);

        IQuery<TEntity, TSpecification> Where(TSpecification specification);

        IQuery<TEntity, TSpecification> OrderBy<TProperty>(
           Expression<Func<TEntity, TProperty>> expression,
           SortOrder sortOrder = SortOrder.Ascending);

        IQuery<TEntity, TSpecification> Include<TProperty>(Expression<Func<TEntity, TProperty>> expression);

        TEntity Single();

        TEntity SingleOrDefault();

        TEntity FirstOrDefault();

        IEnumerable<TEntity> All();

        bool Any();

        IEnumerable<TEntity> Paged(int? pageNumber, int? take);

        long Count();
    }

    
}