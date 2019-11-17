using System;
using Aska.Core.EntityStorage.Abstractions;
using Aska.Core.Storage.Ef;

namespace Aska.Core.EntityStorage.Ef.MariaDb.Tests
{
    public class TestCommandFactory<TEntity> : ICommandFactory where TEntity : class
    {
        private readonly Func<IEntityStorageWriter<TEntity>> _writer;

        public TestCommandFactory(Func<IEntityStorageWriter<TEntity>> writer)
        {
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }
        
        public TCommand GetCommand<TEnt, TCommand, TResult>() where TCommand : ICommand<TEnt, TResult>
        {
            throw new NotImplementedException();
        }

        public T GetCommand<T>() where T : ICommandBase
        {
            throw new NotImplementedException();
        }

        public ICreateCommand<T> GetCreateCommand<T>() where T : class
        {
            VerifyEntityTypeOrThrow<T>();
            
            return (ICreateCommand<T>) new EfDatabaseCreateCommand<TEntity>(_writer());
        }

        public IDeleteCommand<T> GetDeleteCommand<T>() where T : class
        {
            VerifyEntityTypeOrThrow<T>();
            
            return (IDeleteCommand<T>) new EfDatabaseDeleteCommand<TEntity>(_writer());
        }

        public IUpdateCommand<T> GetUpdateCommand<T>() where T : class
        {
            VerifyEntityTypeOrThrow<T>();
            
            return (IUpdateCommand<T>) new EfDatabaseUpdateCommand<TEntity>(_writer());
        }

        public IBulkCreateCommand<T> GetBulkCreateCommand<T>() where T : class
        {
            VerifyEntityTypeOrThrow<T>();
            
            return (IBulkCreateCommand<T>)new EfDatabaseCreateCommand<TEntity>(_writer());
        }

        public IBulkDeleteCommand<T> GetBulkDeleteCommand<T>() where T : class
        {
            VerifyEntityTypeOrThrow<T>();
            
            return (IBulkDeleteCommand<T>)new EfDatabaseDeleteCommand<TEntity>(_writer());
        }

        public IBulkUpdateCommand<T> GetBulkUpdateCommand<T>() where T : class
        {
            VerifyEntityTypeOrThrow<T>();
            
            return (IBulkUpdateCommand<T>)new EfDatabaseUpdateCommand<TEntity>(_writer());
        }

        private void VerifyEntityTypeOrThrow<T>()
        {
            if (typeof(TEntity) != typeof(T)) 
                throw new NotImplementedException($"Only {typeof(TEntity).Name} type is implemented");
        }
    }
}