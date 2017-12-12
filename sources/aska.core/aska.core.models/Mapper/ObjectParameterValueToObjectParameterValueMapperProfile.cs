using aska.core.models.ObjectEntitySchema;

namespace aska.core.models.Mapper
{
    public class ObjectParameterValueToObjectParameterValueMapperProfile : Profile
    {
        public ObjectParameterValueToObjectParameterValueMapperProfile()
        {
            CreateMap<ObjectParameterValue, ObjectParameterValue>()
                .ForMember(m => m.Id, o => o.MapFrom(e => e.Id))
                .ForMember(m => m.LastModifiedBy, o => o.MapFrom(e => e.LastModifiedBy))
                .ForMember(m => m.LastModifiedDateUtc, o => o.MapFrom(e => e.LastModifiedDateUtc))
                .ForMember(m => m.ObjectEntityId, o => o.MapFrom(e => e.ObjectEntityId))
                .ForMember(m => m.ParameterKey, o => o.MapFrom(e => e.ParameterKey))
                .ForMember(m => m.RawValue, o => o.MapFrom(e => e.RawValue));

        }
    }
}