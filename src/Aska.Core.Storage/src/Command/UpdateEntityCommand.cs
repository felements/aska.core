using System;
using System.Threading;
using System.Threading.Tasks;
using Aska.Core.Storage.Abstractions;

namespace Aska.Core.Storage.Command
{
    public class UpdateEntityCommand<T> : UnitOfWorkScopeCommand<T>, IUpdateCommand<T>
        where T : class
    {
        public override async Task ExecuteAsync(T context, CancellationToken ct)
        {
            var uow = GetFromScope();
            uow.Update(context);
            await uow.SaveChangesAsync(ct);
            
            //todo: introduce timetracked entity command
        }

        public UpdateEntityCommand(IServiceProvider scope) : base(scope)
        {
        }
    }
}