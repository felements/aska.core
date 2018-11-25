using System.Threading.Tasks;
using aska.core.common.Data.Entity;
using aska.core.infrastructure.data.CommandQuery.Interfaces;
using aska.core.infrastructure.data.Store;
using Autofac;

namespace aska.core.infrastructure.data.CommandQuery.Command
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

        public virtual Task ExecuteAsync(T context)
        {
            return Task.CompletedTask;
        }
    }
}