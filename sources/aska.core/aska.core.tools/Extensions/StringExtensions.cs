using System;

namespace aska.core.tools.Extensions
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }

        public static string TakeLeft(this string source, int length)
        {
            if (string.IsNullOrWhiteSpace(source)) return source;
            var ln = Math.Min(length, source.Length);

            return source.Substring(0, ln - 1);
        }

        public static string Limit(this string source, int length)
        {
            if (source == null || source.Length <= length) return source;
            return source.Substring(0, length - 1);
        }
    }
}