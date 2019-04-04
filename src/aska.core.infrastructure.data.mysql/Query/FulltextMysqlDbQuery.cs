using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using aska.core.infrastructure.data.CommandQuery.Interfaces;
using aska.core.infrastructure.data.ef.Context;
using aska.core.infrastructure.data.Model;
using aska.core.infrastructure.data.mysql.Context;

namespace aska.core.infrastructure.data.mysql.Query
{
    [Obsolete("Not safe. Should be rewritten")]
    public class FulltextMysqlDbQuery<TEntity, TSpecification> : IQuery<TEntity, TSpecification>
        where TEntity : class, IEntity
        where TSpecification : class, IFulltextMatchSpecification<TEntity>
    {
        private readonly IMysqlDbContextExtendedOperations _ctx;

        public FulltextMysqlDbQuery(IMysqlDbContextExtendedOperations ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }

        private IQueryable<TEntity> Query { get; set; }

        #region Implementation of IQuery<TEntity,in TSpecification>

        public IQuery<TEntity, TSpecification> Where(Expression<Func<TEntity, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IQuery<TEntity, TSpecification> Where(TSpecification specification)
        {
            var tableName = _ctx.GetTableName<TEntity>();
            var selectors = specification.FieldSelectors
                .Select(s => s.ToString().Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault() ?? string.Empty)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToArray();

            var cmd = new FulltextMatchCommand(tableName, selectors, specification.SearchQuery);
            Query = _ctx.ExecuteRawSqlQuery<TEntity>(cmd);

            return this;
        }

        public IQuery<TEntity, TSpecification> OrderBy<TProperty>(Expression<Func<TEntity, TProperty>> expression, SortOrder sortOrder = SortOrder.Ascending)
        {
            throw new NotImplementedException();
        }

        
        public TEntity Single()
        {
            if (Query == null) throw new Exception("Fulltext specification required.");
            return Query.Single();
        }

        public TEntity SingleOrDefault()
        {
            if (Query == null) throw new Exception("Fulltext specification required.");
            return Query.SingleOrDefault();
        }

        public TEntity FirstOrDefault()
        {
            if (Query == null) throw new Exception("Fulltext specification required.");
            return Query.FirstOrDefault();
        }

        public IEnumerable<TEntity> All()
        {
            if (Query == null) throw new Exception("Fulltext specification required.");

            return Query.ToList();
        }

        public bool Any()
        {
            if (Query == null) throw new Exception("Fulltext specification required.");

            return Query.Any();
        }

        public IEnumerable<TEntity> Paged(int? pageNumber, int? take)
        {
            if (Query == null) throw new Exception("Fulltext specification required.");

            return take.HasValue
                ? Query.Skip((pageNumber ?? 0) * take.Value).Take(take.Value).ToList()
                : All();
        }

        public long Count()
        {
            if (Query == null) throw new Exception("Fulltext specification required.");
            return Query.Count();
        }

        #endregion

        private class FulltextMatchCommand
        {
            private const string FulltextMatchCommandTemplate = "select *, match({1}) against('{2}' IN BOOLEAN MODE) as relevance  from {0} where match({1}) against('{2}' IN BOOLEAN MODE);";

            public FulltextMatchCommand(string tblName, string[] columns, string[] query)
            {
                //todo: query text clean up

                if (string.IsNullOrWhiteSpace(tblName)) throw new ArgumentOutOfRangeException(nameof(tblName));
                _tableName = tblName;
                _columns = columns ?? new string[] { };
                _query = query ?? new string[] { };
            }

            private readonly string _tableName;
            private readonly string[] _columns;
            private readonly string[] _query;

            #region Overrides of Object

            public static implicit operator string(FulltextMatchCommand cmd)
            {
                return cmd.ToString();
            }

            public override string ToString()
            {
                return string.Format(FulltextMatchCommandTemplate,
                    _tableName,
                    string.Join(",", _columns),
                    string.Join(" ", _query)
                    );
            }

            #endregion
        }
    }


}