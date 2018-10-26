using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace kd.misc
{
    public static partial class ApplicationExtensions
    {
        private static readonly string Version;
        static ApplicationExtensions()
        {
#if DEBUG
            Version = "dev." + Guid.NewGuid().ToString("N").Substring(0, 4);
            Version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;//todo;
#else
            var ver = Assembly.GetExecutingAssembly().GetName().Version;
            Version = string.Format("{0}.{1}.{2}.{3}", ver.Major, ver.Minor, ver.Build, ver.Revision);
#endif
            
        }

        public static AppEnvironmentInfo GetEnvironment()
        {
            return new AppEnvironmentInfo()
            {
                Architecture = (RuntimeArchitecture)RuntimeInformation.OSArchitecture,
                Framework = RuntimeInformation.FrameworkDescription,
                OsName = RuntimeInformation.OSDescription,
                OsType = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ? RuntimeOsType.Windows
                    : (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                        ? RuntimeOsType.Linux
                        : (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
                            ? RuntimeOsType.Osx
                            : RuntimeOsType.Unknown))
            };
        }

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

        public static CultureInfo DefaultCulture = CultureInfo.GetCultureInfo("ru-RU");
    }
}