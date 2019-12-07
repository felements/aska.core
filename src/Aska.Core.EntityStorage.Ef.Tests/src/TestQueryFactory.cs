using System;
using Aska.Core.EntityStorage.Abstractions;
using Aska.Core.Storage.Ef;

namespace Aska.Core.EntityStorage.Ef.MariaDb.Tests
{
    public class TestQueryFactory<TEntity> : IQueryFactory where TEntity : class
    {
        private readonly Func<IEntityStorageReader<TEntity>> _reader;

        public TestQueryFactory(Func<IEntityStorageReader<TEntity>> reader)
        {
            _reader = reader;
        }
        
        public IQuery<T, IExpressionSpecification<T>> GetQuery<T>() where T : class
        {
            VerifyEntityTypeOrThrow<T>();
            return (IQuery<T, IExpressionSpecification<T>>)new EfDatabaseNoTrackingQuery<TEntity>(_reader());
        }

        public IQuery<T, TSpecification> GetQuery<T, TSpecification>() where T : class
            where TSpecification : ISpecification<T>
        {
            VerifyEntityTypeOrThrow<T>();
            return GetQueryInternal<T, TSpecification>();
        }

        private IQuery<T, TSpecification> GetQueryInternal<T, TSpecification>()
            where TSpecification : ISpecification<T> where T : class
        {
            var queryType = typeof(EfDatabaseNoTrackingQuery<,>).MakeGenericType(typeof(TEntity), typeof(TSpecification));

            return (IQuery<T, TSpecification>) Activator.CreateInstance(queryType, _reader());
        }

        public TQuery GetQuery<TEntity1, TSpecification, TQuery>() where TEntity1 : class where TSpecification : ISpecification<TEntity1> where TQuery : IQuery<TEntity1, TSpecification>
        {
            throw new System.NotImplementedException();
        }
        
        private void VerifyEntityTypeOrThrow<T>()
        {
            if (typeof(TEntity) != typeof(T)) 
                throw new NotImplementedException($"Only {typeof(TEntity).Name} type is implemented");
        }
    }
}