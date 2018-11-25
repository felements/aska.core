namespace aska.core.common.Extensions
{
    public static class AssemblyRootPathProvider//: IRootPathProvider
    {
        public static string GetRootPath()
        {
            var d = AssemblyExtensions.AssemblyDirectory;
            return d;
        }
    }
}