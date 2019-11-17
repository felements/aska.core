using System;
using System.ComponentModel.DataAnnotations;
using Aska.Core.EntityStorage.DemoApp.Specification;

namespace Aska.Core.EntityStorage.Ef.MariaDb.Tests
{
    public interface ITestEntity : IEntity<Guid>
    {
    }
    
    public class TestDbEntity : ITestEntity
    {
        [Obsolete("Only for model binders. Don't use it in your code.", true)]
        public TestDbEntity()
        {
        }

        public TestDbEntity(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
        
        [Key]
        public Guid Id { get; private set; }
        
        public string Name { get; set; }
    }
}