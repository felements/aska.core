using Microsoft.Data.Sqlite;

namespace Aska.Core.EntityStorage.Ef.Sqlite;

public class SqliteConnectionString
{
    private readonly SqliteConnectionStringBuilder _builder = new();

    public SqliteConnectionString WithDataFile(string dataFile)
    {
        _builder.DataSource = dataFile;
        return this;
    }

    public override string ToString() => _builder.ConnectionString;

    public static implicit operator string(SqliteConnectionString connectionString) => connectionString.ToString();

    public static SqliteConnectionString Create() => new();
}
