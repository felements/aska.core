namespace Aska.Core.EntityStorage.Ef.PostgreSql
{
    public class PsqlConnectionString
    {
        private string _server;
        private string _database;
        private string _user;
        private string _password;
        
        public PsqlConnectionString WithServer(string server)
        {
            _server = server;
            return this;
        }

        public PsqlConnectionString WithDatabase(string database)
        {
            _database = database;
            return this;
        }

        public PsqlConnectionString WithUser(string user)
        {
            _user = user;
            return this;
        }

        public PsqlConnectionString WithPassword(string password)
        {
            _password = password;
            return this;
        }

        public override string ToString()
        {
            return $"Host={_server};Database={_database};Username={_user};Password={_password}";
        }

        public static implicit operator string(PsqlConnectionString connStr)
        {
            return connStr.ToString();
        }
        
        public static PsqlConnectionString Create() => new PsqlConnectionString();
    }
}