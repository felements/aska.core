using aska.core.infrastructure.data.ef.Context;

namespace aska.core.infrastructure.data.mysql.Context
{
    public class DefaultMysqlConnectionStringProvider : IConnectionStringProvider
    {
        private readonly string _name;
        private readonly string _host;

        public DefaultMysqlConnectionStringProvider(string name, string host)
        {
            _name = name;
            _host = host;
        }
        
        public string Get()
        {
            return $"server={_host};database={_name};user id={_name};password={_name}";
        }

        public static DefaultMysqlConnectionStringProvider Create(string name, string host = "localhost")
        {
            return new DefaultMysqlConnectionStringProvider(name, host);
        }
    }
}