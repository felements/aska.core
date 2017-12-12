using System;

namespace aska.core.infrastructure.Store.Seed
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SeedOrderAttribute : Attribute
    {
         public int Order { get; set; }

        public SeedOrderAttribute(int order)
        {
            Order = order;
        }
    }
}