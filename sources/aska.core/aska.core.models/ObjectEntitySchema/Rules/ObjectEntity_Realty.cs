using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using aska.core.models.ObjectEntitySchema;

// ReSharper disable once CheckNamespace
namespace ask.realty.contracts.models.ObjectEntitySchema
{
    public partial class ObjectEntity
    {
        public static Expression<Func<aska.core.models.ObjectEntitySchema.ObjectEntity, bool>> RealtyObjectRule =
            (obj) => obj.Type == ObjectType.RealtyApartments || obj.Type == ObjectType.RealtyCottages || obj.Type == ObjectType.RealtyLand;


        public static Func<aska.core.models.ObjectEntitySchema.ObjectEntity, int> OrderSelector = (obj) => obj
            .Values.Let(x=>x, new List<ObjectParameterValue>())
            .Where(v => v.ParameterKey == ObjectParameterKey.Order)
            .Select(v => (int) v.AsTyped(0))
            .FirstOrDefault();
    }
}