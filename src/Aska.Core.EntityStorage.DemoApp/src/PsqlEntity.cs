using Aska.Core.EntityStorage.DemoApp.Specification;

namespace Aska.Core.EntityStorage.DemoApp;

public interface IPsqlEntity : IEntity<Guid>;

public class PsqlEntity : IPsqlEntity
{
    // Used by EF Core when materializing entities.
    private PsqlEntity()
    {
    }

    public PsqlEntity(string data)
    {
        Id = Guid.CreateVersion7();
        Data = data;
    }

    public Guid Id { get; private set; }

    public string Data { get; private set; } = string.Empty;
}
