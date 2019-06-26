using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace aska.core.infrastructure.data.ef.Context
{
    public interface IDbContext
    {
        DbSet<T> GetDbSet<T>() where T : class;
        Task<int> SaveChangesAsync(CancellationToken ct);
        EntityEntry Entry(object o);
    }
}