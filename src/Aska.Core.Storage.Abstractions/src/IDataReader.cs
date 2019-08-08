using System.Linq;

namespace Aska.Core.Storage.Abstractions
{
    public interface IDataReader 
    {
        IQueryable<T> Set<T>() where T: class;
    }
}