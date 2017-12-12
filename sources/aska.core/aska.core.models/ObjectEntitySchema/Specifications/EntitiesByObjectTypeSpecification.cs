using System.Linq;

namespace aska.core.models.ObjectEntitySchema.Specifications
{
    public class EntitiesByObjectTypeSpecification : ExpressionSpecification<ObjectEntity>
    {
        public EntitiesByObjectTypeSpecification(ObjectType type = ObjectType.Unknown) : base(parameter => parameter.Type == type ) {}

        public EntitiesByObjectTypeSpecification(ObjectType[] types) : base(parameter => true)
        {
            if (types == null || !types.Any()) return;

            SpecificationExpression = entity => types.Contains(entity.Type);
        }
    }
}