using System;

namespace Aska.Core.EntityStorage.DemoApp
{
    public interface ISqliteEntity
    {
        Guid Id { get; }
    }
    
    public class SqliteEntity : ISqliteEntity
    {
        protected SqliteEntity()
        {
        }
        
        public SqliteEntity(string description)
        {
            Id = Guid.NewGuid();
            Description = description;
        }
        
        public Guid Id { get; }
        
        public string Description { get; }
    }
}