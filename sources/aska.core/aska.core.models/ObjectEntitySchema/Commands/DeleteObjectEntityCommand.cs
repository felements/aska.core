namespace aska.core.models.ObjectEntitySchema.Commands
{
    public class DeleteObjectEntityCommand : DeleteEntityCommand<ObjectEntity>
    {
        public DeleteObjectEntityCommand(ILifetimeScope scope) : base(scope, false) {}
    }
}