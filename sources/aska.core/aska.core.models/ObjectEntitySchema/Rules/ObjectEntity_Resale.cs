using System;
using System.Linq;
using System.Linq.Expressions;
using aska.core.models.ObjectEntitySchema;

// ReSharper disable once CheckNamespace
namespace ask.realty.contracts.models.ObjectEntitySchema
{
    public partial class ObjectEntity
    {
        public static Expression<Func<aska.core.models.ObjectEntitySchema.ObjectEntity, bool>> ResaleObjectRule =
            (obj) => obj.Type == ObjectType.ResaleApartment || obj.Type == ObjectType.ResaleLand || obj.Type == ObjectType.ResaleHouse;

        public static Func<aska.core.models.ObjectEntitySchema.ObjectEntity, bool> HotOfferRule =
            (obj) => obj.Values.FirstOrDefault(x=>x.ParameterKey == ObjectParameterKey.HotOffer).Let(x=>bool.Parse(x.RawValue),false );

    }
}