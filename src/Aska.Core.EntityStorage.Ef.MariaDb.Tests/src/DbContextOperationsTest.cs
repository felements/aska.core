using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Aska.Core.EntityStorage.Ef.MariaDb.Tests
{
    [Trait("Category","Integration")]
    public class DbContextCrudOperationsTest : IClassFixture<DbContextFixture<TestMariaDbContext>>
    {
        private readonly DbContextFixture<TestMariaDbContext> _contextFixture;

        public DbContextCrudOperationsTest(DbContextFixture<TestMariaDbContext> contextFixture)
        {
            _contextFixture = contextFixture;
        }
        
        [Fact]
        public async Task Command_OnCreateCommand_WithSingleEntity_ShouldResultTrue()
        {
            var result = await _contextFixture
                .CommandFactory
                .GetCreateCommand<TestDbEntity>()
                .ExecuteAsync(new TestDbEntity(Guid.NewGuid().ToString("D")));
            
            Assert.True(result);
        }

        //todo: move to bulk tests
        [Fact]
        public async Task Command_OnCreateCommand_WithManyEntities_ShouldResultEntitiesCount()
        {
            const int count = 10;
            var result = await _contextFixture
                .CommandFactory
                .GetBulkCreateCommand<TestDbEntity>()
                .ExecuteAsync(Enumerable.Range(0, count).Select(i => new TestDbEntity(Guid.NewGuid().ToString("D"))));

            Assert.Equal(count, result);
        }

        [Fact]
        public async Task Command_OnCreateCommand_WithSingleEntity_ShouldQueryCreatedEntity()
        {
            var reference = new TestDbEntity(Guid.NewGuid().ToString("D"));
            
            var created = await _contextFixture
                .CommandFactory
                .GetCreateCommand<TestDbEntity>()
                .ExecuteAsync(reference);

            var loaded = await _contextFixture
                .QueryFactory
                .GetQuery<TestDbEntity, EntityByIdSpecification>()
                .Where(new EntityByIdSpecification(reference.Id))
                .SingleOrDefaultAsync();

            Assert.True(created);
            Assert.NotNull(loaded);
            Assert.Equal(reference.Id, loaded.Id);
            Assert.Equal(reference.Name, loaded.Name);
        }

        [Fact]
        public async Task Command_OnUpdateCommand_ShouldQueryChangedData()
        {
            var reference = new TestDbEntity(Guid.NewGuid().ToString("D"));
            var updatedName = Guid.NewGuid().ToString("D");
            
            await _contextFixture
                .CommandFactory
                .GetCreateCommand<TestDbEntity>()
                .ExecuteAsync(reference);

            reference.Name = updatedName;
            await _contextFixture.CommandFactory
                .GetUpdateCommand<TestDbEntity>()
                .ExecuteAsync(reference);


            var loaded = await _contextFixture.QueryFactory
                .GetQuery<TestDbEntity, EntityByIdSpecification>()
                .Where(new EntityByIdSpecification(reference.Id))
                .SingleOrDefaultAsync();
            
            Assert.NotNull(loaded);
            Assert.Equal(updatedName, loaded.Name);
        }

        [Fact]
        public async Task Command_OnUpdateCommand_WithNotExistingEntity_ShouldFail()
        {
            var reference = new TestDbEntity(Guid.NewGuid().ToString("D"));

            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() =>  _contextFixture
                .CommandFactory
                .GetUpdateCommand<TestDbEntity>()
                .ExecuteAsync(reference));
        }

        [Fact]
        public async Task Command_OnDeleteCommand_WithExistingEntity_ShouldNotQueryAfterDeleted()
        {
            var reference = new TestDbEntity(Guid.NewGuid().ToString("D"));

            bool created = await _contextFixture.CommandFactory.GetCreateCommand<TestDbEntity>().ExecuteAsync(reference);
            bool deleted = await _contextFixture.CommandFactory.GetDeleteCommand<TestDbEntity>().ExecuteAsync(reference);

            var loaded = await _contextFixture.QueryFactory.GetQuery<TestDbEntity, EntityByIdSpecification>()
                .Where(new EntityByIdSpecification(reference.Id))
                .AllAsync();
            
            Assert.True(created);
            Assert.True(deleted);
            Assert.Empty(loaded);
        }

        [Fact]
        public async Task Command_OnDeleteCommand_WithNotExistingEntity_ShouldFail()
        {
            var reference = new TestDbEntity(Guid.NewGuid().ToString("D"));

            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(
                ()=>  _contextFixture.CommandFactory.GetDeleteCommand<TestDbEntity>().ExecuteAsync(reference));
        }

        [Fact]
        public async Task Query_WithExistingEntities_ShouldQueryByCondition()
        {
            var name = Guid.NewGuid().ToString("D");
            const int count = 10;

            await _contextFixture.CommandFactory
                .GetBulkCreateCommand<TestDbEntity>()
                .ExecuteAsync(Enumerable.Range(0, 10).Select(i => new TestDbEntity(name)));

            var entities = (IReadOnlyCollection<TestDbEntity>) await _contextFixture
                .QueryFactory
                .GetQuery<TestDbEntity>()
                .Where(e => e.Name == name)
                .AllAsync();

            Assert.Equal(count, entities.Count());
            Assert.True(entities.All(e=>e.Name == name));
        }
        
        [Fact]
        public async Task Query_WithExistingEntities_ShouldCountSameAmount()
        {
            var name = Guid.NewGuid().ToString("D");
            const int count = 10;

            await _contextFixture.CommandFactory
                .GetBulkCreateCommand<TestDbEntity>()
                .ExecuteAsync(Enumerable.Range(0, 10).Select(i => new TestDbEntity(name)));

            var loaded = await _contextFixture
                .QueryFactory
                .GetQuery<TestDbEntity>()
                .Where(e => e.Name == name)
                .CountAsync();

            Assert.Equal(count, loaded);
        }

        [Fact]
        public async Task Query_WithNotExistingEntity_ShouldQueryNothing()
        {
            var reference = new TestDbEntity(Guid.NewGuid().ToString("D"));

            var loaded = await _contextFixture.QueryFactory
                .GetQuery<TestDbEntity, EntityByIdSpecification>()
                .Where(new EntityByIdSpecification(reference.Id))
                .SingleOrDefaultAsync();
            
            Assert.Null(loaded);
        }
    }
}