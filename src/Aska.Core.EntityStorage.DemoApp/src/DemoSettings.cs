using Aska.Core.EntityStorage.Ef.MariaDb;
using Aska.Core.EntityStorage.Ef.PostgreSql;
using Aska.Core.EntityStorage.Ef.Sqlite;

namespace Aska.Core.EntityStorage.DemoApp;

/// <summary>
/// Connection settings matching the databases in the repository's compose.yaml.
/// </summary>
internal static class DemoSettings
{
    public const string EntitiesAssemblyPrefix = "Aska.Core.EntityStorage.DemoApp";

    public static string MariaDbConnection => MariaDbConnectionString.Create()
        .WithServer("localhost").WithDatabase("askaone").WithUser("askaone").WithPassword("askaone");

    public static string PostgreSqlConnection => PsqlConnectionString.Create()
        .WithServer("localhost").WithDatabase("askaone").WithUser("askaone").WithPassword("askaone");

    public static string SqliteConnection => SqliteConnectionString.Create()
        .WithDataFile(Path.Combine(AppContext.BaseDirectory, "demo.db"));
}
