using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using aska.core.infrastructure.CommandQuery.Interfaces;

namespace aska.core.infrastructure.CommandQuery.Command
{
    public class UpdateEntityCommand<T> : UnitOfWorkScopeCommand<T>
        where T : class, IEntity
    {
        public UpdateEntityCommand(ILifetimeScope scope) : base(scope)
        {
        }

        public override void Execute(T context)
        {
            var uow = GetFromScope();
            uow.SetChangeTracking<T>(true);
            uow.Save(context);
            uow.Commit();
        }

        public void Execute<TProperty>(T context, Expression<Func<T, TProperty>> include)
        {
            var uow = GetFromScope();
            uow.SetChangeTracking<T>(true);
            uow.Save(context, include);
            uow.Commit();
        }

        public void Execute<TProperty_1, TProperty_2>(T context, Expression<Func<T, TProperty_1>> include_1, Expression<Func<T, TProperty_2>> include_2)
        {
            var uow = GetFromScope();
            uow.SetChangeTracking<T>(true);
            uow.Save(context, include_1, include_2);
            uow.Commit();
        }


        public override void Execute(IEnumerable<T> context)
        {
            var uow = GetFromScope();
            
            uow.SetChangeTracking<T>(false);
            foreach (var itm in context)
            {
                uow.Save(itm);
            }
            uow.SetChangeTracking<T>(true);
            uow.Commit();
        }

    }
}