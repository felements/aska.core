using Autofac;
using kd.domainmodel.Entity;
using kd.infrastructure.CommandQuery.Interfaces;
using kd.infrastructure.Store;

namespace kd.infrastructure.CommandQuery.Command
{
    public class UnitOfWorkScopeCommand<T> : ICommand<T> where T : class, IEntity
    {
        protected ILifetimeScope Scope;

        public UnitOfWorkScopeCommand(ILifetimeScope scope)
        {
            Scope = scope;
        }

        public IUnitOfWork GetFromScope()
        {
            return Scope.Resolve<IUnitOfWork>();
        }

        public virtual void Execute(T context)
        {
        }
    }
}