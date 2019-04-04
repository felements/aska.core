using System;
using System.Linq;
using aska.core.common;
using Microsoft.EntityFrameworkCore;

namespace aska.core.infrastructure.data.mysql.Extensions
{
    public static class DbModelBuilderExtensions
    {
        public static void RegisterDerivedTypes<TBase>(this ModelBuilder builder, string assembliesNamePrefix)
        {
            var entityMethod = typeof(ModelBuilder).GetMethods().SingleOrDefault(m => m.IsGenericMethod && m.Name == "Entity" && m.GetParameters().Length == 0);
            if (entityMethod == null) throw new Exception("Model builder should contain 'Entity<>()' method.");

            var types = AssemblyExtensions.GetDerivedTypes<TBase>(assembliesNamePrefix);
            foreach (var type in types)
            {
                var generic = entityMethod.MakeGenericMethod(type);
                var entity = generic.Invoke(builder, null);
            }
        }

    }
}