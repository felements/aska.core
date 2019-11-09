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
        
        public Guid Id { get; private set; }
        
        public string Description { get; private  set; }
    }
}