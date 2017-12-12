using System;
using System.Linq;

namespace aska.core.models.ObjectEntitySchema.Specifications
{
    public class EntitiesByIdSpecification : ExpressionSpecification<ObjectEntity>
    {
        public EntitiesByIdSpecification(Guid id) : base(parameter => parameter.Id == id ) {}

        public EntitiesByIdSpecification(Guid[] ids, bool allIfEmpty = true) : base(parameter => true)
        {
            if (!ids.Any() && allIfEmpty)  return;
            SpecificationExpression = entity => ids.Contains(entity.Id);
        }
    }
}