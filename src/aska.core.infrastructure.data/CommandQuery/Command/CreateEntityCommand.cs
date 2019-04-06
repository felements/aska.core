using System;
using System.Threading.Tasks;
using aska.core.common;

namespace aska.core.infrastructure.data.CommandQuery.Command
{
    public class CreateEntityCommand<T> : UnitOfWorkScopeCommand<T>
        where T : class, IEntity
    {
        public override async Task ExecuteAsync(T context)
        {
            var uow = GetFromScope();
            uow.Save(context);
            await uow.CommitAsync();
        }

        public CreateEntityCommand(IServiceProvider scope) : base(scope)
        {
        }
    }
}