using System.Collections.Generic;
using aska.core.infrastructure.CommandQuery.Interfaces;

namespace aska.core.infrastructure.CommandQuery.Command
{
    public sealed class CreateEntityCommand<T> : UnitOfWorkScopeCommand<T>
        where T : class, IEntity
    {
        public override void Execute(T context)
        {
            var uow = GetFromScope();
            uow.SetChangeTracking<T>(false);
            uow.Save(context);
            uow.Commit();
        }

        public override void Execute(IEnumerable<T> context)
        {
            var uow = GetFromScope();
            uow.SetChangeTracking<T>(false);

            foreach (var itm in context)
            {
                uow.Save(itm);
            }
            //uow.SetChangeTracking<T>(true);
            uow.Commit();
        }

        public CreateEntityCommand(ILifetimeScope scope) : base(scope)
        {
        }
    }
}