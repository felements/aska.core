﻿using System;
using System.Linq.Expressions;
using aska.core.models.ObjectEntitySchema;

// ReSharper disable once CheckNamespace
namespace ask.realty.contracts.models.ObjectEntitySchema
{
    public partial class ObjectEntity
    {
        public static Expression<Func<aska.core.models.ObjectEntitySchema.ObjectEntity, bool>> BrandObjectRule = (obj) => obj.Type == ObjectType.Brand  || obj.Type == ObjectType.ConstructionCompany;
    }
}