using System;
using System.ComponentModel.DataAnnotations;

namespace Aska.Core.EntityStorage.Ef.Tests
{
    public interface ITestEntity<out TKey>
    {
        TKey Id { get; }
    }
    
    public class TestDbEntity : ITestEntity<Guid>
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