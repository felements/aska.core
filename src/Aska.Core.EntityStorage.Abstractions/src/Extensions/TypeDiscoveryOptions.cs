using System;

namespace Aska.Core.EntityStorage.Abstractions.Extensions
{
    public class TypeDiscoveryOptions
    {
        protected TypeDiscoveryOptions()
        {
        }
        
        public TypeDiscoveryOptions(Type baseType, string assemblyNamePrefix, bool forceLoadAssemblies)
        {
            BaseType = baseType;
            AssemblyNamePrefix = assemblyNamePrefix;
            ForceLoadAssemblies = forceLoadAssemblies;
        }
        
        public Type BaseType { get; private set; }
        
        public string AssemblyNamePrefix { get; private set; }
        
        public bool ForceLoadAssemblies { get; private set; }
        
        
        public static Builder<T> ForBaseEntity<T>() where T : class => new Builder<T>();
        
        public class Builder<T> where T: class
        {
            private readonly TypeDiscoveryOptions _options 
                = new TypeDiscoveryOptions(typeof(T), string.Empty, false);

            public Builder<T> ForAssemblies(string assemblyPrefix)
            {
                _options.AssemblyNamePrefix = assemblyPrefix;
                return this;
            }

            public Builder<T> ForceLoadAssemblies()
            {
                _options.ForceLoadAssemblies = true;
                return this;
            }

            public TypeDiscoveryOptions Create()
            {
                return _options;
            }
        }
    }
}