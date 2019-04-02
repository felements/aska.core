using aska.core.infrastructure.data.Model;
using Autofac;

namespace aska.core.infrastructure.data.CommandQuery.Command
{
    public class UpdateEntityCommand<T> : CreateEntityCommand<T>
        where T : class, IEntity
    {
        public UpdateEntityCommand(ILifetimeScope scope) : base(scope)
        {
        }
    }
}