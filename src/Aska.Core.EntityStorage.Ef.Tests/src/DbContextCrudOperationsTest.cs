using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aska.Core.EntityStorage.Abstractions;
using Aska.Core.EntityStorage.Ef.MariaDb.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Aska.Core.EntityStorage.Ef.Tests
{
    public class DbContextCrudOperationsTest<TContext> 
        where TContext : IEntityStorageWriter, IEntityStorageReader, new()
    {
        protected readonly DbContextFixture<TContext> ContextFixture;

        public DbContextCrudOperationsTest(DbContextFixture<TContext> contextFixture)
        {
            ContextFixture = contextFixture;
        }
        
        public virtual async Task Command_OnCreateCommand_WithSingleEntity_ShouldResultTrue()
        {
            var result = await ContextFixture
                .CommandFactory
                .GetCreateCommand<TestDbEntity>()
                .ExecuteAsync(new TestDbEntity(Guid.NewGuid().ToString("D")));
            
            Assert.True(result);
        }

        //todo: move to bulk tests
        public virtual async Task Command_OnCreateCommand_WithManyEntities_ShouldResultEntitiesCount()
        {
            const int count = 10;
            var result = await ContextFixture
                .CommandFactory
                .GetBulkCreateCommand<TestDbEntity>()
                .ExecuteAsync(Enumerable.Range(0, count).Select(i => new TestDbEntity(Guid.NewGuid().ToString("D"))));

            Assert.Equal(count, result);
        }

        public virtual async Task Command_OnCreateCommand_WithSingleEntity_ShouldQueryCreatedEntity()
        {
            var reference = new TestDbEntity(Guid.NewGuid().ToString("D"));
            
            var created = await ContextFixture
                .CommandFactory
                .GetCreateCommand<TestDbEntity>()
                .ExecuteAsync(reference);

            var loaded = await ContextFixture
                .QueryFactory
                .GetQuery<TestDbEntity, EntityByIdSpecification>()
                .Where(new EntityByIdSpecification(reference.Id))
                .SingleOrDefaultAsync();

            Assert.True(created);
            Assert.NotNull(loaded);
            Assert.Equal(reference.Id, loaded.Id);
            Assert.Equal(reference.Name, loaded.Name);
        }

        public virtual async Task Command_OnUpdateCommand_ShouldQueryChangedData()
        {
            var reference = new TestDbEntity(Guid.NewGuid().ToString("D"));
            var updatedName = Guid.NewGuid().ToString("D");
            
            await ContextFixture
                .CommandFactory
                .GetCreateCommand<TestDbEntity>()
                .ExecuteAsync(reference);

            reference.Name = updatedName;
            await ContextFixture.CommandFactory
                .GetUpdateCommand<TestDbEntity>()
                .ExecuteAsync(reference);


            var loaded = await ContextFixture.QueryFactory
                .GetQuery<TestDbEntity, EntityByIdSpecification>()
                .Where(new EntityByIdSpecification(reference.Id))
                .SingleOrDefaultAsync();
            
            Assert.NotNull(loaded);
            Assert.Equal(updatedName, loaded.Name);
        }

        public virtual async Task Command_OnUpdateCommand_WithNotExistingEntity_ShouldFail()
        {
            var reference = new TestDbEntity(Guid.NewGuid().ToString("D"));

            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() =>  ContextFixture
                .CommandFactory
                .GetUpdateCommand<TestDbEntity>()
                .ExecuteAsync(reference));
        }

        public virtual async Task Command_OnDeleteCommand_WithExistingEntity_ShouldNotQueryAfterDeleted()
        {
            var reference = new TestDbEntity(Guid.NewGuid().ToString("D"));

            bool created = await ContextFixture.CommandFactory.GetCreateCommand<TestDbEntity>().ExecuteAsync(reference);
            bool deleted = await ContextFixture.CommandFactory.GetDeleteCommand<TestDbEntity>().ExecuteAsync(reference);

            var loaded = await ContextFixture.QueryFactory.GetQuery<TestDbEntity, EntityByIdSpecification>()
                .Where(new EntityByIdSpecification(reference.Id))
                .AllAsync();
            
            Assert.True(created);
            Assert.True(deleted);
            Assert.Empty(loaded);
        }

        public virtual async Task Command_OnDeleteCommand_WithNotExistingEntity_ShouldFail()
        {
            var reference = new TestDbEntity(Guid.NewGuid().ToString("D"));

            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(
                ()=>  ContextFixture.CommandFactory.GetDeleteCommand<TestDbEntity>().ExecuteAsync(reference));
        }

        public virtual async Task Query_WithExistingEntities_ShouldQueryByCondition()
        {
            var name = Guid.NewGuid().ToString("D");
            const int count = 10;

            await ContextFixture.CommandFactory
                .GetBulkCreateCommand<TestDbEntity>()
                .ExecuteAsync(Enumerable.Range(0, 10).Select(i => new TestDbEntity(name)));

            var entities = (IReadOnlyCollection<TestDbEntity>) await ContextFixture
                .QueryFactory
                .GetQuery<TestDbEntity>()
                .Where(e => e.Name == name)
                .AllAsync();

            Assert.Equal(count, entities.Count());
            Assert.True(entities.All(e=>e.Name == name));
        }
        
        public virtual async Task Query_WithExistingEntities_ShouldCountSameAmount()
        {
            var name = Guid.NewGuid().ToString("D");
            const int count = 10;

            await ContextFixture.CommandFactory
                .GetBulkCreateCommand<TestDbEntity>()
                .ExecuteAsync(Enumerable.Range(0, 10).Select(i => new TestDbEntity(name)));

            var loaded = await ContextFixture
                .QueryFactory
                .GetQuery<TestDbEntity>()
                .Where(e => e.Name == name)
                .CountAsync();

            Assert.Equal(count, loaded);
        }

        public virtual async Task Query_WithNotExistingEntity_ShouldQueryNothing()
        {
            var reference = new TestDbEntity(Guid.NewGuid().ToString("D"));

            var loaded = await ContextFixture.QueryFactory
                .GetQuery<TestDbEntity, EntityByIdSpecification>()
                .Where(new EntityByIdSpecification(reference.Id))
                .SingleOrDefaultAsync();
            
            Assert.Null(loaded);
        }
    }
}