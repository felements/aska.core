using aska.core.infrastructure.data.ef.Context;

namespace aska.core.infrastructure.data.mysql.Context
{
    public class DefaultMysqlConnectionStringProvider : IConnectionStringProvider
    {
        private readonly string _name;

        public DefaultMysqlConnectionStringProvider(string name)
        {
            _name = name;
        }
        
        public string Get()
        {
            return $"server=localhost;database={_name};user id={_name};password={_name}";
        }

        public static DefaultMysqlConnectionStringProvider Create(string name)
        {
            return new DefaultMysqlConnectionStringProvider(name);
        }
    }
}