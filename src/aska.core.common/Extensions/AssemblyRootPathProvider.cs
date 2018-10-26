using Nancy;
using kd.misc;


namespace kd.host
{
    public class AssemblyRootPathProvider: IRootPathProvider
    {
        public string GetRootPath()
        {
            var d = AssemblyExtensions.AssemblyDirectory;
            return d;
        }
    }
}