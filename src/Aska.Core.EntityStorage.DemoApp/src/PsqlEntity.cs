using System;
using System.ComponentModel.DataAnnotations;
using Aska.Core.EntityStorage.DemoApp.Specification;

namespace Aska.Core.EntityStorage.DemoApp
{
    public interface IPsqlEntity : IEntity<Guid>
    {
    }
    
    public class PsqlEntity : IPsqlEntity
    {
        protected PsqlEntity()
        {
        }
        
        public PsqlEntity(string data)
        {
            Id = Guid.NewGuid();
            Data = data;
        }
        
        [Key]
        public Guid Id { get; private set; }
        
        public string Data { get; private  set; }
    }
}