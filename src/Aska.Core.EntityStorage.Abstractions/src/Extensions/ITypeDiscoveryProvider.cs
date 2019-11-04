using System;

namespace Aska.Core.EntityStorage.Abstractions.Extensions
{
    public interface ITypeDiscoveryProvider
    {
        /// <summary>
        /// Get types derived from the base one that are defined in the assemblies with specified prefix.
        /// </summary>
        /// <param name="baseType">Base type for the discovery</param>
        /// <param name="assemblyPrefix">Assembly name prefix to filter out the area where to discover</param>
        /// <param name="forceLoadAssemblies">Force load assemblies before discovery</param>
        /// <returns>Discovered types that are derived from the base type</returns>
        Type[] Discover(Type baseType, string assemblyPrefix, bool forceLoadAssemblies = false);
    }
}