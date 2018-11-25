using System;

namespace aska.core.common.Extensions
{
    public class EnumValue<T> where T : struct
    {
        public static T TryParse(string value)
        {
            Enum.TryParse(value, true, out T result);
            return result;
        }

    }
}