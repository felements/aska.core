using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace aska.core.tools.Extensions
{
    public static class PathExtensions
    {
        private static string ReplacePathSeparators(string path, char separator)
        {
            if (string.IsNullOrEmpty(path)) return string.Empty;
            var separatorString = separator.ToString();

            return path.Replace('\\', separator).Replace('/', separator)
                .Replace(separatorString + separatorString + separatorString, separatorString)
                .Replace(separatorString + separatorString, separatorString);
        }

        public static string Combine(params string[] args)
        {
            if (args == null || !args.Any()) return string.Empty;

            return "/" + string.Join("/", args.Select(p => p.Trim('/')));
        }

        public static string GetAssemblyDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }
}