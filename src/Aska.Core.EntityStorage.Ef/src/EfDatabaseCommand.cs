using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aska.Core.EntityStorage.Abstractions;

namespace Aska.Core.Storage.Ef
{
    public class EfDatabaseCommand<TEntity, TResult>: ICommand<TEntity, TResult>
        where TEntity : class
    {
        protected readonly IEntityStorageWriter<TEntity> Writer;

        protected EfDatabaseCommand(IEntityStorageWriter<TEntity> writer)
        {
            Writer = writer;
        }
        
        public virtual Task<TResult> ExecuteAsync(TEntity context, CancellationToken ct = default)
        {
            return Task.FromResult(default(TResult));
        }
    }

    internal class EfDatabaseCreateCommand<TEntity> : 
        EfDatabaseCommand<TEntity, bool>, ICreateCommand<TEntity>, IBulkCreateCommand<TEntity>
        where TEntity : class
    {
        public EfDatabaseCreateCommand(IEntityStorageWriter<TEntity> writer) : base(writer)
        {
        }

        public override async Task<bool> ExecuteAsync(TEntity context, CancellationToken ct = default)
        {
            Writer.Add(context);
            return (await Writer.SaveAsync(ct) == 1);
        }

        public async Task<int> ExecuteAsync(IEnumerable<TEntity> context, CancellationToken ct = default)
        {
            Writer.Add(context);
            return await Writer.SaveAsync(ct);
        }
    }
    
    internal class EfDatabaseUpdateCommand<TEntity> : 
        EfDatabaseCommand<TEntity, bool>, IUpdateCommand<TEntity>, IBulkUpdateCommand<TEntity>
        where TEntity : class
    {
        public EfDatabaseUpdateCommand(IEntityStorageWriter<TEntity> writer) : base(writer)
        {
        }

        public override async Task<bool> ExecuteAsync(TEntity context, CancellationToken ct = default)
        {
            Writer.Update(context);
            return (await Writer.SaveAsync(ct) == 1);
        }

        public async Task<int> ExecuteAsync(IEnumerable<TEntity> context, CancellationToken ct = default)
        {
            Writer.Update(context);
            return await Writer.SaveAsync(ct);
        }
    }
    
    internal class EfDatabaseDeleteCommand<TEntity> : 
        EfDatabaseCommand<TEntity, bool>, IDeleteCommand<TEntity>, IBulkDeleteCommand<TEntity>
        where TEntity : class
    {
        public EfDatabaseDeleteCommand(IEntityStorageWriter<TEntity> writer) : base(writer)
        {
        }

        public override async Task<bool> ExecuteAsync(TEntity context, CancellationToken ct = default)
        {
            Writer.Remove(context);
            return (await Writer.SaveAsync(ct) == 1);
        }

        public async Task<int> ExecuteAsync(IEnumerable<TEntity> context, CancellationToken ct = default)
        {
            Writer.Remove(context);
            return await Writer.SaveAsync(ct);
        }
    }
}