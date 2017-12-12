using System.Collections.Generic;

namespace aska.core.models.ObjectEntitySchema.Specifications
{
    public class InteractionChannelByIdentifierSpecification : EntitiesByParameterValueExactMatchSpecification
    {
        public InteractionChannelByIdentifierSpecification(string identifier) 
            : base(new Dictionary<ObjectParameterKey, string>() { { ObjectParameterKey.Identifier, identifier }}, true)
        {
        }
    }
}