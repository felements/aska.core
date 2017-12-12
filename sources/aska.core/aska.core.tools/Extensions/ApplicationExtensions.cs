using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace aska.core.tools.Extensions
{
    public static class ApplicationExtensions
    {
        private static readonly string Version;
        static ApplicationExtensions()
        {
#if DEBUG
            Version =  "dev." + Guid.NewGuid().ToString("N").Substring(0, 4);
#else
            var ver = Assembly.GetExecutingAssembly().GetName().Version;
            Version = string.Format("{0}.{1}.{2}.{3}", ver.Major, ver.Minor, ver.Build, ver.Revision);
#endif

        }

        public static ApplicationEnvironment GetEnvironment()
        {
            if (Type.GetType("Mono.Runtime") != null)
            {
                return ApplicationEnvironment.Mono;
            }
            return ApplicationEnvironment.WindowsNative;
            //TODO
        }


        //TODO: use semver
        public static string GetVersion()
        {
            return Version;
        }

        public static string CreateStorePath(Guid id, string storePath, string filename)
        {
            var storeFileName = id.ToString("D") + Path.GetExtension(filename ?? string.Empty);
            var path = Environment.CurrentDirectory
                + storePath.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar)
                + storeFileName;

            return path;
        }

        public static CultureInfo RuCulture = CultureInfo.GetCultureInfo("ru-RU");



        #region loadavg

        private static readonly Regex LoadavgExpression = new Regex("(?<current>[0-9]+\\.[0-9]+) \\s+ (?<short>[0-9]+\\.[0-9]+) \\s+ (?<long>[0-9]+\\.[0-9]+)", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
        private static readonly Regex CpusCountExpression = new Regex("CPU\\(s\\):\\s+(?<core_count>[0-9]+)\\s+", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);


        public static LoadavgNormalizedResult GetSystemLoadNormalized()
        {
            var ex = new ShellExecute();

            var resultLoad = ex.Execute("cat /proc/loadavg", new []{ApplicationEnvironment.Mono});
            var resultCpus = ex.Execute("lscpu", new[] { ApplicationEnvironment.Mono });
            if (string.IsNullOrWhiteSpace(resultLoad.Output) || string.IsNullOrWhiteSpace(resultCpus.Output)) return null;

            var loadavg = LoadavgExpression.Match(resultLoad.Output);
            var cpus = CpusCountExpression.Match(resultCpus.Output);

            if (!loadavg.Success || !cpus.Success) return null;

            int coresCount = 0;
            float loadCurrent, loadShort, loadLong;
            if (!int.TryParse(cpus.Groups["core_count"].Value, out coresCount)) return null;
            if (!float.TryParse(loadavg.Groups["current"].Value, out loadCurrent)) return null;
            if (!float.TryParse(loadavg.Groups["short"].Value, out loadShort)) return null;
            if (!float.TryParse(loadavg.Groups["long"].Value, out loadLong)) return null;

            var result = new LoadavgNormalizedResult()
            {
                Current = (float)Math.Round(loadCurrent / coresCount, 2),
                Short = (float)Math.Round(loadShort / coresCount, 2),
                Long = (float)Math.Round(loadLong / coresCount, 2)
            };
            return result;
        }

        public class LoadavgNormalizedResult
        {
            public float Current { get; set; }
            public float Short { get; set; }
            public float Long { get; set; }
        }
        #endregion


    }
}