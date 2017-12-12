using System;
using System.IO;
using System.Reflection;

namespace aska.core.infrastructure.mysql.Migrations
{
    public class ApplyDbScripts : IDataSeed
    {
        public void Seed(IDbContext context)
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            var baseDir = Path.GetDirectoryName(path) + Path.DirectorySeparatorChar +  "Migrations";
            foreach (var sqlScript in Directory.GetFiles(baseDir, "*.sql", SearchOption.AllDirectories))
            {
                context.ExecuteSqlCommand(File.ReadAllText(sqlScript));
            }
        }
    }
}