using System.Collections.Generic;
using System.Linq;

namespace aska.core.models.ObjectEntitySchema.Specifications
{
    public class ParameterByObjectTypeSpecification : ExpressionSpecification<ObjectParameter>
    {
        public ParameterByObjectTypeSpecification(ObjectType type = ObjectType.Unknown) : base(parameter => parameter.ObjectType == type ) {}
        public ParameterByObjectTypeSpecification(IEnumerable<ObjectType> types) : base(parameter => types.Contains(parameter.ObjectType)) {}
    }
}