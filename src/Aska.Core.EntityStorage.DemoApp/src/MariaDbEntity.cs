using System;

namespace Aska.Core.EntityStorage.DemoApp
{
    public interface IMariaDbEntity
    {
        Guid Id { get; }
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
        
        public Guid Id { get; private set; }
        
        public string Name { get; private set; }
    }
}