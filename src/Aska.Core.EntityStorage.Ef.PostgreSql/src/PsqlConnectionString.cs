using Npgsql;

namespace Aska.Core.EntityStorage.Ef.PostgreSql;

public class PsqlConnectionString
{
    private readonly NpgsqlConnectionStringBuilder _builder = new();

    public PsqlConnectionString WithServer(string server)
    {
        _builder.Host = server;
        return this;
    }

    public PsqlConnectionString WithDatabase(string database)
    {
        _builder.Database = database;
        return this;
    }

    public PsqlConnectionString WithUser(string user)
    {
        _builder.Username = user;
        return this;
    }

    public PsqlConnectionString WithPassword(string password)
    {
        _builder.Password = password;
        return this;
    }

    public override string ToString() => _builder.ConnectionString;

    public static implicit operator string(PsqlConnectionString connectionString) => connectionString.ToString();

    public static PsqlConnectionString Create() => new();
}
