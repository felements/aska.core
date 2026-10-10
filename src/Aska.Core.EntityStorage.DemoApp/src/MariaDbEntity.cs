using Aska.Core.EntityStorage.DemoApp.Specification;

namespace Aska.Core.EntityStorage.DemoApp;

public interface IMariaDbEntity : IEntity<Guid>;

public class MariaDbEntity : IMariaDbEntity
{
    // Used by EF Core when materializing entities.
    private MariaDbEntity()
    {
    }

    public MariaDbEntity(string name)
    {
        Id = Guid.CreateVersion7();
        Name = name;
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;
}
