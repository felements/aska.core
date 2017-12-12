using System;

namespace aska.core.infrastructure.Resource
{
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = true)]
    public class TitleAttribute : Attribute
    {
        //TODO: implement culture-specific values

        public TitleAttribute(string title)
        {
            Title = title;
        }

        public string Title { get; set; }
    }
}