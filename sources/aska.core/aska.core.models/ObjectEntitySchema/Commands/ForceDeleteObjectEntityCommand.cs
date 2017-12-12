using System.Collections.Generic;

namespace aska.core.models.ObjectEntitySchema.Commands
{
    public class ForceDeleteObjectEntityCommand : UnitOfWorkScopeCommand<ObjectEntity>
    {
        public ForceDeleteObjectEntityCommand(ILifetimeScope scope) : base(scope) {}

        public override void Execute(ObjectEntity context)
        {
            var uow = GetFromScope();
            uow.SetChangeTracking<ObjectEntity>(false);

            if (context.Values != null)
            {
                foreach (var value in context.Values)
                {
                    uow.Delete(value);
                }
            }
            uow.Delete(context);

            uow.Commit();
        }

        public override void Execute(IEnumerable<ObjectEntity> context)
        {
            var uow = GetFromScope();
            uow.SetChangeTracking<ObjectEntity>(false);

            foreach (var entity in context)
            {
                if (entity.Values != null)
                {
                    foreach (var value in entity.Values)
                    {
                        uow.Delete(value);
                    }
                }
                uow.Delete(entity);
            }

            uow.Commit();
        }
    }
}