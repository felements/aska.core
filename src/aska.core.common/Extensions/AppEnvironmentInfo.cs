namespace kd.misc
{
    public static partial class ApplicationExtensions
    {
        public struct AppEnvironmentInfo
        {
            public RuntimeOsType OsType { get; set; }
            public string OsName { get; set; }
            public RuntimeArchitecture Architecture { get; set; }
            public string Framework { get; set; }
        }
    }
}