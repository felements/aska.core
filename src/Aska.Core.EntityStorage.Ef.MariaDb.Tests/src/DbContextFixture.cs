using Aska.Core.EntityStorage.Abstractions;
using Aska.Core.Storage.Ef;

namespace Aska.Core.EntityStorage.Ef.MariaDb.Tests
{
    public class DbContextFixture<TContext> where TContext : IEntityStorageWriter, IEntityStorageReader, new()
    {
        public DbContextFixture()
        {
            CommandFactory = new TestCommandFactory<TestDbEntity>(
                () => new EntityStorageWriterContextProxy<TestDbEntity, TContext>(new TContext()));
            QueryFactory = new TestQueryFactory<TestDbEntity>(
                () => new EntityStorageReaderContextProxy<TestDbEntity,TContext>(new TContext()));
        }

        public IQueryFactory QueryFactory { get; }
        
        public ICommandFactory CommandFactory { get; }
    }
}