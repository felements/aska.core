using System.Collections.Generic;
using System.Linq;

namespace aska.core.models.ObjectEntitySchema.Specifications
{
    public class EntitiesByParameterValueExactMatchSpecification : ExpressionSpecification<ObjectEntity>
    {
        public EntitiesByParameterValueExactMatchSpecification(IDictionary<ObjectParameterKey, string> parameterValues,  bool allValuesRequiredToMatch = true) 
            : base(entity => true)
        {
            if (parameterValues == null || !parameterValues.Any()) return;

            var plain = parameterValues
                .Where(x=>!string.IsNullOrWhiteSpace(x.Value))
                .Select(x => (x.Key + "=" + x.Value).TrimEnd().ToLower()).ToList();
            // AND logic
            if (allValuesRequiredToMatch) SpecificationExpression = entity => plain.All(   fltr => entity.Values.Select(val=>(val.ParameterKey + "=" + val.RawValue).ToLower()).Contains(fltr)   );
            // OR logic
            else SpecificationExpression = entity => entity.Values.Any(val => plain.Contains((val.ParameterKey + "=" + val.RawValue).ToLower()));
        }
    }
}