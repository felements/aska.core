using System;
using System.Threading;
using System.Threading.Tasks;
using Aska.Core.EntityStorage.Abstractions;

namespace Aska.Core.Storage.Command
{
    public class DeleteEntityCommand<T> : UnitOfWorkScopeCommand<T>, IDeleteCommand<T> where T : class
    {
        public DeleteEntityCommand(IServiceProvider provider) : base(provider)
        {
        }

        public override async Task ExecuteAsync(T context, CancellationToken ct)
        {
            //todo: introduce fakeDeleted extended command
            
//            if (context is IEntityFakeDeleted fakeDeleted)
//            {
//                fakeDeleted.IsDeleted = true;
//
//                if (context is IEntityTimeTracked timeTracked)
//                {
//                    timeTracked.UpdatedAt = DateTime.UtcNow;
//                }
//                
//                uow.Update(context);
//            }
//            else
//            {
//                uow.Delete(context);
//            }


            var uow = GetFromScope();
            uow.Remove(context);
            await uow.SaveChangesAsync(ct);
        }

    }
}