using System;
using System.Threading;
using System.Threading.Tasks;
using aska.core.common;

namespace aska.core.infrastructure.data.CommandQuery.Command
{
    public class UpdateEntityCommand<T> : UnitOfWorkScopeCommand<T>
        where T : class, IEntity
    {
        public override async Task ExecuteAsync(T context, CancellationToken ct)
        {
            var uow = GetFromScope();
            
            if (context is IEntityTimeTracked timeTracked)
            {
                timeTracked.UpdatedAt = DateTime.UtcNow;
            }
            
            uow.Update(context);
            await uow.CommitAsync(ct);
        }

        public UpdateEntityCommand(IServiceProvider scope) : base(scope)
        {
        }
    }
}