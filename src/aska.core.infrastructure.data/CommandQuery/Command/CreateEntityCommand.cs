using Autofac;
using kd.domainmodel.Entity;

namespace kd.infrastructure.CommandQuery.Command
{
    public class CreateEntityCommand<T> : UnitOfWorkScopeCommand<T>
        where T : class, IEntity
    {
        public override void Execute(T context)
        {
            GetFromScope().Save(context);
            GetFromScope().Commit();
        }

        public CreateEntityCommand(ILifetimeScope scope) : base(scope)
        {
        }
    }
}