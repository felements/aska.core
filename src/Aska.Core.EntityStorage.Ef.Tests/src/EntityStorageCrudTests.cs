using Aska.Core.EntityStorage.Abstractions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Aska.Core.EntityStorage.Ef.Tests;

/// <summary>
/// CRUD tests that every provider must pass. Each provider test project derives a sealed class from this one.
/// </summary>
public abstract class EntityStorageCrudTests<TFixture, TContext>(TFixture fixture)
    where TFixture : EntityStorageFixture<TContext>
    where TContext : DbContext
{
    private const int BulkCount = 10;

    private ICommandFactory Commands => fixture.CommandFactory;

    private IQueryFactory Queries => fixture.QueryFactory;

    private static CancellationToken Ct => TestContext.Current.CancellationToken;

    private static TestDbEntity NewEntity(string? name = null) => new(name ?? Guid.NewGuid().ToString("D"));

    private Task<TestDbEntity?> FindAsync(Guid id) =>
        Queries.GetQuery<TestDbEntity, EntityByIdSpecification>()
            .Where(new EntityByIdSpecification(id))
            .SingleOrDefaultAsync(Ct);

    [Fact]
    public async Task Command_OnCreateCommand_WithSingleEntity_ShouldResultTrue()
    {
        var result = await Commands.GetCreateCommand<TestDbEntity>().ExecuteAsync(NewEntity(), Ct);

        Assert.True(result);
    }

    [Fact]
    public async Task Command_OnCreateCommand_WithManyEntities_ShouldResultEntitiesCount()
    {
        var entities = Enumerable.Range(0, BulkCount).Select(_ => NewEntity());

        var result = await Commands.GetBulkCreateCommand<TestDbEntity>().ExecuteAsync(entities, Ct);

        Assert.Equal(BulkCount, result);
    }

    [Fact]
    public async Task Command_OnCreateCommand_WithSingleEntity_ShouldQueryCreatedEntity()
    {
        var reference = NewEntity();

        var created = await Commands.GetCreateCommand<TestDbEntity>().ExecuteAsync(reference, Ct);
        var loaded = await FindAsync(reference.Id);

        Assert.True(created);
        Assert.NotNull(loaded);
        Assert.Equal(reference.Id, loaded.Id);
        Assert.Equal(reference.Name, loaded.Name);
    }

    [Fact]
    public async Task Command_OnUpdateCommand_ShouldQueryChangedData()
    {
        var reference = NewEntity();
        var updatedName = Guid.NewGuid().ToString("D");
        await Commands.GetCreateCommand<TestDbEntity>().ExecuteAsync(reference, Ct);

        reference.Name = updatedName;
        await Commands.GetUpdateCommand<TestDbEntity>().ExecuteAsync(reference, Ct);
        var loaded = await FindAsync(reference.Id);

        Assert.NotNull(loaded);
        Assert.Equal(updatedName, loaded.Name);
    }

    [Fact]
    public async Task Command_OnUpdateCommand_WithNotExistingEntity_ShouldFail()
    {
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(
            () => Commands.GetUpdateCommand<TestDbEntity>().ExecuteAsync(NewEntity(), Ct));
    }

    [Fact]
    public async Task Command_OnDeleteCommand_WithExistingEntity_ShouldNotQueryAfterDeleted()
    {
        var reference = NewEntity();

        var created = await Commands.GetCreateCommand<TestDbEntity>().ExecuteAsync(reference, Ct);
        var deleted = await Commands.GetDeleteCommand<TestDbEntity>().ExecuteAsync(reference, Ct);
        var loaded = await Queries.GetQuery<TestDbEntity, EntityByIdSpecification>()
            .Where(new EntityByIdSpecification(reference.Id))
            .AllAsync(Ct);

        Assert.True(created);
        Assert.True(deleted);
        Assert.Empty(loaded);
    }

    [Fact]
    public async Task Command_OnDeleteCommand_WithNotExistingEntity_ShouldFail()
    {
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(
            () => Commands.GetDeleteCommand<TestDbEntity>().ExecuteAsync(NewEntity(), Ct));
    }

    [Fact]
    public async Task Query_WithExistingEntities_ShouldQueryByCondition()
    {
        var name = Guid.NewGuid().ToString("D");
        await Commands.GetBulkCreateCommand<TestDbEntity>()
            .ExecuteAsync(Enumerable.Range(0, BulkCount).Select(_ => NewEntity(name)), Ct);

        var entities = (await Queries.GetQuery<TestDbEntity>().Where(e => e.Name == name).AllAsync(Ct)).ToList();

        Assert.Equal(BulkCount, entities.Count);
        Assert.All(entities, e => Assert.Equal(name, e.Name));
    }

    [Fact]
    public async Task Query_WithExistingEntities_ShouldCountSameAmount()
    {
        var name = Guid.NewGuid().ToString("D");
        await Commands.GetBulkCreateCommand<TestDbEntity>()
            .ExecuteAsync(Enumerable.Range(0, BulkCount).Select(_ => NewEntity(name)), Ct);

        var count = await Queries.GetQuery<TestDbEntity>().Where(e => e.Name == name).CountAsync(Ct);

        Assert.Equal(BulkCount, count);
    }

    [Fact]
    public async Task Query_WithNotExistingEntity_ShouldQueryNothing()
    {
        var loaded = await FindAsync(Guid.NewGuid());

        Assert.Null(loaded);
    }

    [Fact]
    public async Task Query_WithOrderBy_ShouldReturnSortedEntities()
    {
        var prefix = Guid.NewGuid().ToString("N");
        var names = new[] { $"{prefix}-c", $"{prefix}-a", $"{prefix}-b" };
        await Commands.GetBulkCreateCommand<TestDbEntity>().ExecuteAsync(names.Select(n => NewEntity(n)), Ct);

        var loaded = await Queries.GetQuery<TestDbEntity>()
            .Where(e => e.Name.StartsWith(prefix))
            .OrderBy(e => e.Name, SortOrder.Descending)
            .AllAsync(Ct);

        Assert.Equal(names.OrderDescending(StringComparer.Ordinal), loaded.Select(e => e.Name));
    }
}
