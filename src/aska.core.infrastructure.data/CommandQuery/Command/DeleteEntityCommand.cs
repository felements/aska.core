using System;
using System.Threading;
using System.Threading.Tasks;
using aska.core.common;

namespace aska.core.infrastructure.data.CommandQuery.Command
{
    public class DeleteEntityCommand<T> : UnitOfWorkScopeCommand<T>
        where T : class, IEntity
    {
        public DeleteEntityCommand(IServiceProvider scope) : base(scope)
        {
        }

        public override async Task ExecuteAsync(T context, CancellationToken ct)
        {
            var uow = GetFromScope();

            if (context is IEntityFakeDeleted fakeDeleted)
            {
                fakeDeleted.IsDeleted = true;

                if (context is IEntityTimeTracked timeTracked)
                {
                    timeTracked.UpdatedAt = DateTime.UtcNow;
                }
                
                uow.Update(context);
            }
            else
            {
                uow.Delete(context);
            }

            await uow.CommitAsync(ct);
        }

    }
}