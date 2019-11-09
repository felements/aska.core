using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aska.Core.EntityStorage.Abstractions;

namespace Aska.Core.Storage.Ef
{
    public interface IEntityStorageContextInitializer
    {
        Task InitializeAsync(CancellationToken ct = default);
    }

    internal class EntityStorageContextInitializer : IEntityStorageContextInitializer
    {
        private readonly IEnumerable<IEntityStorageInitialize> _contexts;

        public EntityStorageContextInitializer(
            IEnumerable<IEntityStorageInitialize> contexts)
        {
            _contexts = contexts;
        }

        public async Task InitializeAsync(CancellationToken ct = default)
        {
            foreach (var ctx in _contexts)
            {
                await ctx.InitializeAsync(ct);
            }
        }
    }
}