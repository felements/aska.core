using System;
using aska.core.infrastructure.data.Model;

namespace aska.core.infrastructure.data.CommandQuery.Command
{
    public class UpdateEntityCommand<T> : CreateEntityCommand<T>
        where T : class, IEntity
    {
        public UpdateEntityCommand(IServiceProvider scope) : base(scope)
        {
        }
    }
}