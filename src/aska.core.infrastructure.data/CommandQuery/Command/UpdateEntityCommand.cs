using System;
using aska.core.common;

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