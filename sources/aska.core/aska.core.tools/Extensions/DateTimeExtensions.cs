using System;

namespace aska.core.tools.Extensions
{
    public class DateTimeExtensions
    {
        public static DateTimeOffset FromUnixTimeMilliseconds(long milliseconds, TimeSpan offset)
        {
            if (milliseconds < -62135596800000L || milliseconds > 253402300799999L)
                throw new ArgumentOutOfRangeException(nameof(milliseconds),
                    string.Format("Should be between {0} and {1}", (object) -62135596800000L,
                        (object) 253402300799999L));
            return new DateTimeOffset(milliseconds * 10000L + 621355968000000000L, offset);
        }
    }
}

