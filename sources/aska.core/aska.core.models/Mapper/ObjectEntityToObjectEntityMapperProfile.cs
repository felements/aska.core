using aska.core.models.ObjectEntitySchema;

namespace aska.core.models.Mapper
{
    public class ObjectEntityToObjectEntityMapperProfile : Profile
    {
        public ObjectEntityToObjectEntityMapperProfile()
        {
            CreateMap<ObjectEntity, ObjectEntity>()
                .ForMember(m => m.Id, o => o.MapFrom(e => e.Id))
                .ForMember(m => m.IsDeleted, o => o.MapFrom(e => e.IsDeleted))
                .ForMember(m => m.LastModifiedBy, o => o.MapFrom(e => e.LastModifiedBy))
                .ForMember(m => m.LastModifiedDateUtc, o => o.MapFrom(e => e.LastModifiedDateUtc))
                .ForMember(m => m.Title, o => o.MapFrom(e => e.Title))
                .ForMember(m => m.Type, o => o.MapFrom(e => e.Type))
                .ForMember(m => m.Values,
                    o => o.ResolveUsing(new MergeCollectionValueResolver<ObjectEntity, ObjectParameterValue>(entity => entity.Values) ));
        }
    }
}