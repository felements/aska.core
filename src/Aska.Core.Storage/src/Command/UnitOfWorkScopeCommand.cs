using System;
using System.Threading;
using System.Threading.Tasks;
using Aska.Core.EntityStorage.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Aska.Core.Storage.Command
{
    public class UnitOfWorkScopeCommand<T> : ICommand<T> where T : class
    {
        private readonly IServiceScope _scope;

        protected UnitOfWorkScopeCommand(IServiceProvider provider)
        {
            _scope = provider.CreateScope();
        }

        protected IUnitOfWork GetFromScope()
        {
            return _scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        }

        public virtual Task ExecuteAsync(T context, CancellationToken ct)
        {
            return Task.CompletedTask;
        }
    }
}