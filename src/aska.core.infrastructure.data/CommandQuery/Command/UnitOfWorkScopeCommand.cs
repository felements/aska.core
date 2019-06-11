using System;
using System.Threading;
using System.Threading.Tasks;
using aska.core.common;
using aska.core.infrastructure.data.CommandQuery.Interfaces;
using aska.core.infrastructure.data.Store;
using Microsoft.Extensions.DependencyInjection;

namespace aska.core.infrastructure.data.CommandQuery.Command
{
    public class UnitOfWorkScopeCommand<T> : ICommand<T> where T : class, IEntity
    {
        protected readonly IServiceProvider Scope;

        public UnitOfWorkScopeCommand(IServiceProvider scope)
        {
            Scope = scope;
        }

        public IUnitOfWork GetFromScope()
        {
            return Scope.GetRequiredService<IUnitOfWork>();
        }

        public virtual Task ExecuteAsync(T context, CancellationToken ct)
        {
            return Task.CompletedTask;
        }
    }
}