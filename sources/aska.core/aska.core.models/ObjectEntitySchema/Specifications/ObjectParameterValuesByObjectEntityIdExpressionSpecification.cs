using System;

namespace aska.core.models.ObjectEntitySchema.Specifications
{
    public class ObjectParameterValuesByObjectEntityIdExpressionSpecification: ExpressionSpecification<ObjectParameterValue>
    {
        public ObjectParameterValuesByObjectEntityIdExpressionSpecification(Guid relatedEntityId, ObjectParameterKey? parameterKey = null) : base(Rules.ObjectParameterValue.ActiveRule)
        {
            SpecificationExpression = SpecificationExpression.AndAlso(value => value.ObjectEntityId == relatedEntityId);

            if (parameterKey.HasValue)
                SpecificationExpression =
                    SpecificationExpression.AndAlso(value => value.ParameterKey == parameterKey.Value);

        }
    }
}