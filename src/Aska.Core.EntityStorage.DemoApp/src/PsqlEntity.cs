using System;

namespace Aska.Core.EntityStorage.DemoApp
{
    public interface IPsqlEntity
    {
        Guid Id { get; }
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
        
        public Guid Id { get; private set; }
        
        public string Data { get; private  set; }
    }
}