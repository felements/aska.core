using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace kd.domainmodel
{
    public static class Args
    {
        public static int KeyMaxLength = 50;
        public static int ValueMaxLength = 1024;
        public const char SequenceSeparator = ',';

        private static readonly Regex AllowedKeyChars = new Regex(@"[^a-zа-я0-9-\.]+",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

        private static readonly Regex AllowedValueChars = new Regex(@"[^a-zа-я0-9-\.\[\]\{\},""]+",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

        public static T EnsureNotNull<T>(T value, string paramName = "")
        {
            if (value == null) throw new ArgumentNullException(paramName);
            return value;
        }

        public static string EnsureNotEmpty(string value, string paramName = "")
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentOutOfRangeException(paramName);
            return value;
        }

        /// <summary>
        /// Converts string to the Guid array
        /// </summary>
        /// <exception cref="FormatException">In case of wrong format for one of the values</exception>
        /// <returns>An array of Guid values</returns>
        public static Guid[] Parse(string raw)
        {
            var result = new Guid[] { };

            if (string.IsNullOrWhiteSpace(raw)) return result;
            result = raw
                .Split(new[] {SequenceSeparator}, StringSplitOptions.RemoveEmptyEntries)
                .Select(Guid.Parse)
                .ToArray();

            return result;
        }

        /// <summary>
        /// limit value length and remove not allowed chars for preset 'key'
        /// </summary>
        /// <returns>Normalized lowercase value</returns>
        public static string NormalizeKey(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return string.Empty;

            var key = AllowedKeyChars.Replace(raw.ToLowerInvariant(), string.Empty).Trim();
            return key.Length > KeyMaxLength ? key.Substring(0, KeyMaxLength) : key;
        }

        /// <summary>
        /// limit value length and remove not allowed chars for preset 'value'
        /// </summary>
        /// <returns>Normalized lowercase value</returns>
        public static string NormalizeValue(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return string.Empty;

            var value = AllowedValueChars.Replace(raw.ToLowerInvariant(), string.Empty).Trim();
            return value.Length > ValueMaxLength ? value.Substring(0, ValueMaxLength) : value;
        }
    }
}