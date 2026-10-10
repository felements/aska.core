using Aska.Core.EntityStorage.DemoApp.Specification;

namespace Aska.Core.EntityStorage.DemoApp;

public interface ISqliteEntity : IEntity<Guid>;

public class SqliteEntity : ISqliteEntity
{
    // Used by EF Core when materializing entities.
    private SqliteEntity()
    {
    }

    public SqliteEntity(string description)
    {
        Id = Guid.CreateVersion7();
        Description = description;
    }

    public Guid Id { get; private set; }

    public string Description { get; private set; } = string.Empty;
}
