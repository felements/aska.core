using System;
using System.Collections.Generic;
using aska.core.infrastructure.CommandQuery.Interfaces;

namespace aska.core.infrastructure.CommandQuery.Command
{
    public class TruncateCommand<T> : UnitOfWorkScopeCommand<T>
        where T : class, IEntity
    {

        public TruncateCommand( ILifetimeScope scope) : base(scope)
        {
        }


        public void Execute()
        {
            GetFromScope().Truncate<T>();
        }

        public override void Execute(IEnumerable<T> context)
        {
            throw new NotSupportedException();
        }

        public override void Execute(T context)
        {
            throw new NotSupportedException();
        }
    }
}