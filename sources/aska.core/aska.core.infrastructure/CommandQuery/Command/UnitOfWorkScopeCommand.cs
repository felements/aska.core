using System.Collections.Generic;
using aska.core.infrastructure.CommandQuery.Interfaces;
using aska.core.infrastructure.Store;

namespace aska.core.infrastructure.CommandQuery.Command
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

        public virtual void Execute(IEnumerable<T> context)
        {
        }
    }

    
}