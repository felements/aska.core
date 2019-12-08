using System.Threading.Tasks;
using Aska.Core.EntityStorage.Ef.Tests;
using Xunit;

namespace Aska.Core.EntityStorage.Ef.MariaDb.Tests
{
    
    [Trait("Category","Integration")]
    public class MariaDbCrudOperationsTests : DbContextCrudOperationsTest<TestMariaDbContext>, IClassFixture<DbContextFixture<TestMariaDbContext>> 
    {
        public MariaDbCrudOperationsTests(DbContextFixture<TestMariaDbContext> contextFixture) : base(contextFixture)
        {
        }

        [Fact]
        public override Task Command_OnUpdateCommand_ShouldQueryChangedData()
        {
            return base.Command_OnUpdateCommand_ShouldQueryChangedData();
        }

        [Fact]
        public override Task Query_WithExistingEntities_ShouldCountSameAmount()
        {
            return base.Query_WithExistingEntities_ShouldCountSameAmount();
        }

        [Fact]
        public override Task Query_WithExistingEntities_ShouldQueryByCondition()
        {
            return base.Query_WithExistingEntities_ShouldQueryByCondition();
        }

        [Fact]
        public override Task Query_WithNotExistingEntity_ShouldQueryNothing()
        {
            return base.Query_WithNotExistingEntity_ShouldQueryNothing();
        }

        [Fact]
        public override Task Command_OnCreateCommand_WithSingleEntity_ShouldResultTrue()
        {
            return base.Command_OnCreateCommand_WithSingleEntity_ShouldResultTrue();
        }

        [Fact]
        public override Task Command_OnDeleteCommand_WithNotExistingEntity_ShouldFail()
        {
            return base.Command_OnDeleteCommand_WithNotExistingEntity_ShouldFail();
        }

        [Fact]
        public override Task Command_OnUpdateCommand_WithNotExistingEntity_ShouldFail()
        {
            return base.Command_OnUpdateCommand_WithNotExistingEntity_ShouldFail();
        }

        [Fact]
        public override Task Command_OnCreateCommand_WithManyEntities_ShouldResultEntitiesCount()
        {
            return base.Command_OnCreateCommand_WithManyEntities_ShouldResultEntitiesCount();
        }

        [Fact]
        public override Task Command_OnCreateCommand_WithSingleEntity_ShouldQueryCreatedEntity()
        {
            return base.Command_OnCreateCommand_WithSingleEntity_ShouldQueryCreatedEntity();
        }

        [Fact]
        public override Task Command_OnDeleteCommand_WithExistingEntity_ShouldNotQueryAfterDeleted()
        {
            return base.Command_OnDeleteCommand_WithExistingEntity_ShouldNotQueryAfterDeleted();
        }
    }
}