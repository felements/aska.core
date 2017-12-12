using System;

namespace aska.core.models.ObjectEntitySchema.Security
{
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = true)]
    public class IsPublicAttribute : Attribute
    {
        public IsPublicAttribute()
        {
        }
    }
}