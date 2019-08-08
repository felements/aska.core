using System.Linq;
using Aska.Core.Storage.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Aska.Core.Storage.Ef.MariaDb
{
    public class MariaDbContextBase : DbContext, IDataReader
    {
        public MariaDbContextBase(DbContextOptions options) : base(options)
        {
        }
        
        IQueryable<T> IDataReader.Set<T>() => Set<T>();
    }
}