using Autofac;
using kd.domainmodel.Entity;

namespace kd.infrastructure.CommandQuery.Command
{
    public class DeleteEntityCommand<T> : UnitOfWorkScopeCommand<T>
        where T : class, IEntity
    {
        public DeleteEntityCommand( ILifetimeScope scope) : base(scope)
        {
        }

        public override void Execute(T context)
        {
            var fakeDeleted = context as IEntityFakeDeleted;
            if (fakeDeleted != null)
            {
                fakeDeleted.IsDeleted = true;
                GetFromScope().Save(context);
            }
            else
            {
                GetFromScope().Delete(context);
            }
            
            GetFromScope().Commit();
        }
    }
}