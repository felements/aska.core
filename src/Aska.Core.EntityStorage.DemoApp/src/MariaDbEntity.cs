using System;
using System.ComponentModel.DataAnnotations;
using Aska.Core.EntityStorage.DemoApp.Specification;

namespace Aska.Core.EntityStorage.DemoApp
{
    public interface IMariaDbEntity: IEntity<Guid>
    {
    }

    public class MariaDbEntity : IMariaDbEntity
    {
        protected MariaDbEntity()
        {
        }

        public MariaDbEntity(string name)
        {
            Id = Guid.Empty;
            Name = name;
        }
        
        [Key]
        public Guid Id { get; private set; }
        
        public string Name { get; private set; }
    }
}