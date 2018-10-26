using Autofac;
using kd.domainmodel.Entity;

namespace kd.infrastructure.CommandQuery.Command
{
    public class UpdateEntityCommand<T> : CreateEntityCommand<T>
        where T : class, IEntity
    {
        public UpdateEntityCommand(ILifetimeScope scope) : base(scope)
        {
        }
    }
}