namespace Aska.Core.EntityStorage.Ef.MariaDb
{
    public class MariaDbConnectionString
    {
        private string _server;
        private string _database;
        private string _user;
        private string _password;
        
        public MariaDbConnectionString WithServer(string server)
        {
            _server = server;
            return this;
        }

        public MariaDbConnectionString WithDatabase(string database)
        {
            _database = database;
            return this;
        }

        public MariaDbConnectionString WithUser(string user)
        {
            _user = user;
            return this;
        }

        public MariaDbConnectionString WithPassword(string password)
        {
            _password = password;
            return this;
        }
        
        public override string ToString()
        {
            return $"server={_server};database={_database};user id={_user};password={_password}";
        }

        public static implicit operator string(MariaDbConnectionString connStr)
        {
            return connStr.ToString();
        }
        
        public static MariaDbConnectionString Create() => new MariaDbConnectionString();
    }
}