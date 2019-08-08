using System;
using System.Threading;
using System.Threading.Tasks;
using Aska.Core.Storage.Abstractions;

namespace Aska.Core.Storage.Command
{
    public class CreateEntityCommand<T> : UnitOfWorkScopeCommand<T>, ICreateCommand<T>
        where T : class
    {
        public override async Task ExecuteAsync(T context, CancellationToken ct)
        {
            var uow = GetFromScope();
            uow.Add(context);
            await uow.SaveChangesAsync(ct);
            
            //too: introduce timetracked item create command
        }

        public CreateEntityCommand(IServiceProvider provider) : base(provider)
        {
        }
    }
}