using System;
using System.Collections.Generic;
using System.Linq;
using aska.core.infrastructure.CommandQuery.Interfaces;

namespace aska.core.infrastructure.CommandQuery.Command
{
    public class DeleteEntityCommand<T> : UnitOfWorkScopeCommand<T>
        where T : class, IEntity
    {
        private readonly bool _forceDelete;

        public DeleteEntityCommand( ILifetimeScope scope, bool forceDelete = false ) : base(scope)
        {
            _forceDelete = forceDelete;
        }


        public void Execute(IEnumerable<Guid> ids)
        {
            var uow = GetFromScope();
            var fakeDelete = !_forceDelete && typeof(T).GetInterfaces().Any(x => x == typeof(IEntityFakeDeleted));
            
            if (fakeDelete)
            {
                throw new NotSupportedException();
            }
            else
            {
                uow.SetChangeTracking<T>(false);
                foreach (var id in ids)
                {
                    uow.Delete<T>(id);
                }
                //uow.SetChangeTracking<T>(true);
            }
            uow.Commit();
        }

        public override void Execute(IEnumerable<T> context)
        {
            var fakeDelete = !_forceDelete && typeof(T).GetInterfaces().Any(x => x == typeof(IEntityFakeDeleted));

            var uow = GetFromScope();
            if (fakeDelete)
            {
                foreach (var entity in context)
                {
                    ((IEntityFakeDeleted) entity).IsDeleted = true;
                    uow.Save(entity);
                }
            }
            else
            {
                uow.SetChangeTracking<T>(false);
                foreach (var entity in context)
                {
                    uow.Delete(entity);
                }
                //uow.SetChangeTracking<T>(true);
            }
            uow.Commit();
        }

        public override void Execute(T context)
        {
            var uow = GetFromScope();

            var fakeDeleted = context as IEntityFakeDeleted;
            if (fakeDeleted != null && !_forceDelete)
            {
                fakeDeleted.IsDeleted = true;
                uow.Save(context);
            }
            else
            {
                uow.Delete(context);
            }
            
            uow.Commit();
        }
    }
}