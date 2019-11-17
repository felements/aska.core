using System;
using System.ComponentModel.DataAnnotations;
using Aska.Core.EntityStorage.DemoApp.Specification;

namespace Aska.Core.EntityStorage.DemoApp
{
    public interface ISqliteEntity : IEntity<Guid>
    {
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
        
        [Key]
        public Guid Id { get; private set; }
        
        public string Description { get; private  set; }
    }
}