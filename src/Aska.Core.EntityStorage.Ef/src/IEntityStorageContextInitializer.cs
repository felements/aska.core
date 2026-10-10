using Aska.Core.EntityStorage.Abstractions;

namespace Aska.Core.EntityStorage.Ef;

public interface IEntityStorageContextInitializer
{
    Task InitializeAsync(CancellationToken ct = default);
}

internal sealed class EntityStorageContextInitializer(IEnumerable<IEntityStorageInitialize> contexts) : IEntityStorageContextInitializer
{
    public async Task InitializeAsync(CancellationToken ct = default)
    {
        foreach (var context in contexts)
        {
            await context.InitializeAsync(ct);
        }
    }
}
