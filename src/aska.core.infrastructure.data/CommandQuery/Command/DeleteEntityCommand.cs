using System;
using System.Threading.Tasks;
using aska.core.infrastructure.data.Model;

namespace aska.core.infrastructure.data.CommandQuery.Command
{
    public class DeleteEntityCommand<T> : UnitOfWorkScopeCommand<T>
        where T : class, IEntity
    {
        public DeleteEntityCommand( IServiceProvider scope) : base(scope)
        {
        }

        public override async Task ExecuteAsync(T context)
        {
            var uow = GetFromScope();

            if (context is IEntityFakeDeleted fakeDeleted)
            {
                fakeDeleted.IsDeleted = true;
                uow.Save(context);
            }
            else
            {
                uow.Delete(context);
            }

            await uow.CommitAsync();
        }

    }
}