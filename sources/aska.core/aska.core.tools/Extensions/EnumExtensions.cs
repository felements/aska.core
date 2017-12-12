using System;
using System.Linq;

namespace aska.core.tools.Extensions
{
    public static class EnumExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this Enum value)
            where TAttribute : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            var attr =  (TAttribute) type.GetField(name).GetCustomAttributes(typeof(TAttribute), false).SingleOrDefault();
            return attr;
        }

        public static T[] GetAttributes<T>(Enum enumValue) where T : Attribute
        {
            var memberInfo = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();
            if (memberInfo != null)
            {
                var attributes = memberInfo.GetCustomAttributes(typeof(T), false).Cast<T>().ToArray();
                return attributes;
            }
            return null;
        }
    }
}