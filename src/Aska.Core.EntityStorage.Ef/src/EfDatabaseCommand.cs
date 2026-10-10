using Aska.Core.EntityStorage.Abstractions;

namespace Aska.Core.EntityStorage.Ef;

public abstract class EfDatabaseCommand<TEntity, TResult>(IEntityStorageWriter<TEntity> writer) : ICommand<TEntity, TResult>
    where TEntity : class
{
    protected IEntityStorageWriter<TEntity> Writer { get; } = writer;

    public abstract Task<TResult> ExecuteAsync(TEntity context, CancellationToken ct = default);
}

internal sealed class EfDatabaseCreateCommand<TEntity>(IEntityStorageWriter<TEntity> writer) :
    EfDatabaseCommand<TEntity, bool>(writer), ICreateCommand<TEntity>, IBulkCreateCommand<TEntity>
    where TEntity : class
{
    public override async Task<bool> ExecuteAsync(TEntity context, CancellationToken ct = default)
    {
        Writer.Add(context);
        return await Writer.SaveAsync(ct) == 1;
    }

    public async Task<int> ExecuteAsync(IEnumerable<TEntity> context, CancellationToken ct = default)
    {
        Writer.Add(context);
        return await Writer.SaveAsync(ct);
    }
}

internal sealed class EfDatabaseUpdateCommand<TEntity>(IEntityStorageWriter<TEntity> writer) :
    EfDatabaseCommand<TEntity, bool>(writer), IUpdateCommand<TEntity>, IBulkUpdateCommand<TEntity>
    where TEntity : class
{
    public override async Task<bool> ExecuteAsync(TEntity context, CancellationToken ct = default)
    {
        Writer.Update(context);
        return await Writer.SaveAsync(ct) == 1;
    }

    public async Task<int> ExecuteAsync(IEnumerable<TEntity> context, CancellationToken ct = default)
    {
        Writer.Update(context);
        return await Writer.SaveAsync(ct);
    }
}

internal sealed class EfDatabaseDeleteCommand<TEntity>(IEntityStorageWriter<TEntity> writer) :
    EfDatabaseCommand<TEntity, bool>(writer), IDeleteCommand<TEntity>, IBulkDeleteCommand<TEntity>
    where TEntity : class
{
    public override async Task<bool> ExecuteAsync(TEntity context, CancellationToken ct = default)
    {
        Writer.Remove(context);
        return await Writer.SaveAsync(ct) == 1;
    }

    public async Task<int> ExecuteAsync(IEnumerable<TEntity> context, CancellationToken ct = default)
    {
        Writer.Remove(context);
        return await Writer.SaveAsync(ct);
    }
}
