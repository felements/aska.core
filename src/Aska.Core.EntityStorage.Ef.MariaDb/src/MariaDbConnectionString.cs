using MySqlConnector;

namespace Aska.Core.EntityStorage.Ef.MariaDb;

public class MariaDbConnectionString
{
    private readonly MySqlConnectionStringBuilder _builder = new();

    public MariaDbConnectionString WithServer(string server)
    {
        _builder.Server = server;
        return this;
    }

    public MariaDbConnectionString WithDatabase(string database)
    {
        _builder.Database = database;
        return this;
    }

    public MariaDbConnectionString WithUser(string user)
    {
        _builder.UserID = user;
        return this;
    }

    public MariaDbConnectionString WithPassword(string password)
    {
        _builder.Password = password;
        return this;
    }

    public override string ToString() => _builder.ConnectionString;

    public static implicit operator string(MariaDbConnectionString connectionString) => connectionString.ToString();

    public static MariaDbConnectionString Create() => new();
}
