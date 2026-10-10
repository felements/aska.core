namespace Aska.Core.EntityStorage.Ef.Tests;

/// <summary>
/// Marker for the entities the test contexts discover.
/// </summary>
public interface ITestEntity
{
    Guid Id { get; }
}

public class TestDbEntity : ITestEntity
{
    // Used by EF Core when materializing entities.
    private TestDbEntity()
    {
    }

    public TestDbEntity(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

    public Guid Id { get; private set; }

    public string Name { get; set; } = string.Empty;
}
