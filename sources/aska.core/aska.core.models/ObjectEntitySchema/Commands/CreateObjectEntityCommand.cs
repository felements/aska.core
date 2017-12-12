using System.Collections.Generic;

namespace aska.core.models.ObjectEntitySchema.Commands
{
    public class CreateObjectEntityCommand : UnitOfWorkScopeCommand<ObjectEntity>
    {
        public CreateObjectEntityCommand(ILifetimeScope scope) : base(scope) {}

        public override void Execute(ObjectEntity context)
        {
            var uow = GetFromScope();
            uow.SetChangeTracking<ObjectEntity>(false);

            uow.Save(context);
            if (context.Values != null)
            {
                foreach (var value in context.Values)
                {
                    uow.Save(value);
                }
            }
            uow.SetChangeTracking<ObjectEntity>(true);
            uow.Commit();
        }

        public override void Execute(IEnumerable<ObjectEntity> context)
        {
            var uow = GetFromScope();
            uow.SetChangeTracking<ObjectEntity>(false);
            foreach (var entity in context)
            {
                uow.Save(entity);
                if (entity.Values != null)
                {
                    foreach (var value in entity.Values)
                    {
                        uow.Save(value);
                    }
                }
            }

            uow.SetChangeTracking<ObjectEntity>(true);
            uow.Commit();
        }
    }
}