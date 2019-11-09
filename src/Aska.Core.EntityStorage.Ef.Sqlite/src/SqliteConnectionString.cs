namespace Aska.Core.Storage.Ef.Sqlite
{
    public class SqliteConnectionString
    {
        private string _data;

        public SqliteConnectionString WithDataFile(string data)
        {
            _data = data;
            return this;
        }
        
        public override string ToString()
        {
            return $"Data source={_data}";
        }

        public static implicit operator string(SqliteConnectionString connStr)
        {
            return connStr.ToString();
        }
        
        public static SqliteConnectionString Create() => new SqliteConnectionString();
    }
}