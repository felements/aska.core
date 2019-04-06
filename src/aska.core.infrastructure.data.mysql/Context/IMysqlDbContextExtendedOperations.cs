using System.Linq;
using aska.core.common;
using aska.core.infrastructure.data.ef.Context;

namespace aska.core.infrastructure.data.mysql.Context
{
    public interface IMysqlDbContextExtendedOperations : IDbContext
    {
        string GetTableName<T>() where T : class, IEntity;
        
        void TruncateTable<T>() where T : class, IEntity;
        
        void ExecuteRawSqlCommand(string command);
        
        IQueryable<T> ExecuteRawSqlQuery<T>(string query, params object[] parameters) where T : class;
    }
}