using System;
using System.Threading;
using System.Threading.Tasks;
using aska.core.common;

namespace aska.core.infrastructure.data.CommandQuery.Command
{
    public class CreateEntityCommand<T> : UnitOfWorkScopeCommand<T>
        where T : class, IEntity
    {
        public override async Task ExecuteAsync(T context, CancellationToken ct)
        {
            var uow = GetFromScope();

            if (context is IEntityTimeTracked timeTracked)
            {
                timeTracked.CreatedAt = DateTime.UtcNow;
                timeTracked.UpdatedAt = DateTime.UtcNow;
            }
            
            uow.Add(context);
            await uow.CommitAsync(ct);
        }

        public CreateEntityCommand(IServiceProvider scope) : base(scope)
        {
        }
    }
}