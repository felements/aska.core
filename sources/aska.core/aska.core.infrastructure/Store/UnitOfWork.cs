using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using aska.core.infrastructure.CommandQuery.Interfaces;

namespace aska.core.infrastructure.Store
{
    internal class UnitOfWork : IUnitOfWork
    {
        //private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly List<object> _includeDefinitions = new List<object>();
        private readonly IIndex<DbContextKey, IDbContext> _ctxIndex;
        private IDbContext _ctx;

        public Guid Id;

        public UnitOfWork(IIndex<DbContextKey, IDbContext> ctxs)
        {
            _ctxIndex = ctxs;
            Id = Guid.NewGuid();
        }

        public void SetChangeTracking<TEntity>(bool enabled) where TEntity : class, IEntity
        {
            if (_ctx == null) _ctx = _ctxIndex.Choose(typeof(TEntity));

            var changed = _ctx.AutoDetectChangesEnabled != enabled;
            _ctx.AutoDetectChangesEnabled = enabled;
            if (enabled && changed) _ctx.DetectChanges();
        }


        Task IUnitOfWork.CommitAsync()
        {
            if (_ctx == null) throw new Exception("Context not selected");
            return CommitAsync();
        }

        void IUnitOfWork.Commit()
        {
            if (_ctx == null) throw new Exception("Context not selected");
            _ctx.SaveChanges();
        }

        public void Save<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            if (_ctx == null) _ctx = _ctxIndex.Choose(typeof(TEntity));
            var set = _ctx.GetDbSet<TEntity>();

            SaveInternal(entity);
        }

        public void Save<TEntity, TProperty_1>(TEntity entity, Expression<Func<TEntity, TProperty_1>> include_1) where TEntity : class, IEntity
        {
            Include(include_1);
            SaveInternal(entity);
        }

        public void Save<TEntity, TProperty_1, TProperty_2>(TEntity entity, Expression<Func<TEntity, TProperty_1>> include_1, Expression<Func<TEntity, TProperty_2>> include_2) where TEntity : class, IEntity
        {
            Include(include_1);
            Include(include_2);
            SaveInternal(entity);
        }

        private void SaveInternal<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            if (_ctx == null) _ctx = _ctxIndex.Choose(typeof(TEntity));

            IncludeExpressionDef<TEntity>[] includes = _includeDefinitions
                .Where(x => x is IncludeExpressionDef<TEntity>)
                .Cast<IncludeExpressionDef<TEntity>>()
                .ToArray();

            IQueryable<TEntity> set = _ctx.GetDbSet<TEntity>();
            set = includes.Aggregate(set, (current, include) => current.Include(include.Path));

            var dbentity = (TEntity)set.FirstOrDefault(entity.CompareIdExpression());
            if (dbentity != null)
            {
                if (dbentity == entity) return;

                _ctx.Entry(dbentity).CurrentValues.SetValues(entity);


                foreach (var includeExpressionDef in includes)
                {
                    var newEntityPropertyValue = includeExpressionDef.Getter(entity);
                    var dbEntityPropertyValue = includeExpressionDef.Getter(dbentity);


                    if (newEntityPropertyValue is IEnumerable)
                    {
                        var dbValues =  ((IEnumerable) dbEntityPropertyValue).Cast<IEntity>().ToDictionary(x => x.Id);
                        var newValues = new HashSet<Guid>(((IEnumerable) newEntityPropertyValue).Cast<IEntity>().Select(x => x.Id));

                        var dbRemove = dbValues.Keys.Except(newValues).ToList();
                        var dbAppend = newValues.Except(dbValues.Keys).ToList();

                        var collectionItemType = ((IEnumerable)newEntityPropertyValue).GetType().GetGenericArguments()[0];

                        // add new nested items
                        if (dbAppend.Any())
                        {
                            var collectionAddMethod = dbEntityPropertyValue.GetType().GetMethod("Add");

                            var nestedSet = _ctx.GetDbSet(collectionItemType);
                            foreach (Guid addId in dbAppend)
                            {
                                var nestedEntity = nestedSet.Find(addId);
                                collectionAddMethod.Invoke(dbEntityPropertyValue, new object[]
                                {
                                    nestedEntity
                                });
                            }
                        }
                        
                        // remove
                        if (dbRemove.Any())
                        {
                            var collectionRemoveMethod = dbEntityPropertyValue.GetType().GetMethod("Remove");
                            foreach (var removeId in dbRemove)
                            {
                                collectionRemoveMethod.Invoke(dbEntityPropertyValue, new[] { dbValues[removeId]  });
                            }
                        }
                        
                    }
                    else
                    {
                        //todo: call EF's SetValues() method
                        throw new NotImplementedException();
                    }
                }
                
            }
            else
            {
                _ctx.Entry(entity).State = EntityState.Added;
            }
        }
        
        public void Delete<TEntity>(Guid id) where TEntity : class, IEntity
        {
            if (_ctx == null) _ctx = _ctxIndex.Choose(typeof(TEntity));

            var dbSet = _ctx.GetDbSet<TEntity>();
            var entity = dbSet.SingleOrDefault(x => x.Id == id);
            if (entity != null) dbSet.Remove(entity);
        }

        public void Truncate<TEntity>() where TEntity : class, IEntity
        {
            if (_ctx == null) _ctx = _ctxIndex.Choose(typeof(TEntity));

            _ctx.Truncate<TEntity>();
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            if (_ctx == null) _ctx = _ctxIndex.Choose(typeof(TEntity));

            var set = _ctx.GetDbSet<TEntity>();
            var dbentity = (TEntity) set.FirstOrDefault(entity.CompareIdExpression());
            if (dbentity != null) set.Remove(dbentity);
        }

        public int Commit()
        {
            if (_ctx == null) throw new Exception("Context not selected");

            return _ctx.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            if (_ctx == null) throw new Exception("Context not selected");
            return await _ctx.SaveChangesAsync();
        }


        public void Include<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression)
        {
            var boxedParam = Expression.Parameter(typeof(object));
            var castedParam = Expression.Convert(boxedParam, typeof(TProperty));
            var assignProperty = Expression.Assign(propertyExpression.Body, castedParam);

            var assign = Expression.Lambda<Action<TEntity, object>>(
                assignProperty,
                propertyExpression.Parameters[0],
                boxedParam);

            var def = new IncludeExpressionDef<TEntity>
            {
                Getter = AddBox(propertyExpression).Compile(),
                Setter = assign.Compile(),
                Path = string.Join(".", propertyExpression.ToString().Split(new [] {"."}, StringSplitOptions.RemoveEmptyEntries).Skip(1))
            };

            _includeDefinitions.Add(def);
        }

        public class IncludeExpressionDef<TEntity>
        {
            public Action<TEntity, object> Setter { get; set; }
            public Func<TEntity, object> Getter { get; set; }
            public string Path { get; set; }
        }

        private static Expression<Func<TInput, object>> AddBox<TInput, TOutput> (Expression<Func<TInput, TOutput>> expression)
        {
            // Add the boxing operation, but get a weakly typed expression
            Expression converted = Expression.Convert(expression.Body, typeof(object));
            // Use Expression.Lambda to get back to strong typing
            return Expression.Lambda<Func<TInput, object>>(converted, expression.Parameters);
        }
    }
} 