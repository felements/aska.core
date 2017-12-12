using System;
using System.Collections.Generic;
using System.Linq;
using aska.core.models.ObjectEntitySchema;
using aska.core.models.Variants;

namespace aska.core.models.Mapper
{
    public class ObjectEntityToPublicMenuItemMapperProfile : Profile
    {
        public ObjectEntityToPublicMenuItemMapperProfile()
        {
            CreateMap<ObjectEntity, PublicMenuItem>()
                .ForMember(m => m.Id, e => e.MapFrom(o => o.Id))
                .ForMember(m => m.Order, e => e.ResolveUsing((entity, item) => entity.ValueByKey(ObjectParameterKey.Order).AsTyped(-1)))
                .ForMember(m => m.Title, e => e.ResolveUsing((entity, item) => entity[ObjectParameterKey.ShortTitle]))
                .ForMember(m => m.Path, e => e.ResolveUsing((entity, item) => entity[ObjectParameterKey.Alias]?? entity.Id.ToString("D") ))
                .ForSourceMember(nameof(ObjectEntity.Type), o => o.Ignore())
                .ForSourceMember(nameof(ObjectEntity.IsDeleted), o => o.Ignore())
                .ForSourceMember(nameof(ObjectEntity.LastModifiedBy), o => o.Ignore())
                .ForSourceMember(nameof(ObjectEntity.LastModifiedDateUtc), o => o.Ignore())
                .ForSourceMember(nameof(ObjectEntity.Values), o => o.Ignore())
                ;

            CreateMap<IGrouping<ObjectType, ObjectEntity>, PublicMenuItem>()
                .ForMember(m => m.Id, e => e.UseValue(Guid.Empty))
                .ForMember(m => m.Order, e => e.MapFrom(o => o.Key))
                .ForMember(m => m.Path, e => e.UseValue(string.Empty))
                .ForMember(m => m.Title, e => e.ResolveUsing(entities => ObjectTypeDisplayName[entities.Key]))

                .ForMember(m => m.Items, e => e.MapFrom(o => o))
                .AfterMap((entities, item) => item.Items = item.Items.OrderBy(i=>i.Order).ToList() );

        }
        private static readonly Dictionary<ObjectType, string> ObjectTypeDisplayName = new Dictionary<ObjectType, string>()
        {
            { ObjectType.RealtyApartments, "Квартиры"},
            { ObjectType.RealtyCottages, "Коттеджи"},
            { ObjectType.RealtyLand, "Участки"}
        };
    }
}