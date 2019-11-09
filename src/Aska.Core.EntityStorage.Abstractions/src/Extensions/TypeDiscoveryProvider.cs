using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Aska.Core.EntityStorage.Abstractions.Extensions
{
    public class TypeDiscoveryProvider<TContext> : ITypeDiscoveryProvider<TContext> 
        where TContext : IStorageContext
    {
        private readonly Lazy<Type[]> _discoveredTypes;
        public TypeDiscoveryProvider(TypeDiscoveryOptions options, ITypeDiscoveryProvider discoveryProvider)
        {
            _discoveredTypes = new Lazy<Type[]>(
                () => discoveryProvider.Discover(options.BaseType, options.AssemblyNamePrefix, options.ForceLoadAssemblies),
                LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public Type[] Discover() => _discoveredTypes.Value;
    }

    public class TypeDiscoveryProvider : ITypeDiscoveryProvider
    {
        public Type[] Discover(Type baseType, string assemblyPrefix, bool forceLoadAssemblies)
        {
            return DiscoverInternal(baseType, assemblyPrefix, forceLoadAssemblies);
        }
        
        private static Type[] DiscoverInternal(Type baseType, string assemblyPrefix, bool forceLoadAssemblies = false)
        {
            if (forceLoadAssemblies) ForceLoadAssembliesSafe(assemblyPrefix);
            
            var derivedTypes = GetDerivedTypes(baseType, assemblyPrefix).ToArray();
            return derivedTypes;
        }
        
        private static IEnumerable<Type> GetDerivedTypes(Type baseType, string assembliesNamePrefix)
        {
            var assemblies = GetAssemblies(assembliesNamePrefix);

            var result = new List<Type>();
            foreach (var assembly in assemblies)
            {
                result.AddRange(assembly
                    .GetExportedTypes()
                    .Where(tp => baseType.IsAssignableFrom(tp) && tp != baseType));
            }
            return result;
        }

        private static IEnumerable<Assembly> GetAssemblies(string assembliesNamePrefix) =>
            AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.FullName.StartsWith(assembliesNamePrefix, StringComparison.InvariantCultureIgnoreCase));

        private static void ForceLoadAssembliesSafe(string assemblyNamePrefix)
        {
            try
            {
                var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
                var loadedPaths = loadedAssemblies
                    .Where(x => x.FullName.StartsWith(assemblyNamePrefix))
                    .Select(a => a.Location).ToArray();

                var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
                var toLoad = referencedPaths
                    .Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase))
                    .Where(x => Path.GetFileName(x)
                        .StartsWith(assemblyNamePrefix, StringComparison.InvariantCultureIgnoreCase))
                    .ToList();
                toLoad.ForEach(path =>
                {
                    try
                    {
                        AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path));
                    }
                    catch
                    {
                        // ignored
                    }
                });
            }
            catch
            {
                // ignored
            }
        }


        
    }
}