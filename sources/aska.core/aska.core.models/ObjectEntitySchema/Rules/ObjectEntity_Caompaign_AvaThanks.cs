using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using aska.core.models.ObjectEntitySchema;

// ReSharper disable once CheckNamespace
namespace ask.realty.contracts.models.ObjectEntitySchema
{
    public partial class ObjectEntity
    {
        public static Expression<Func<aska.core.models.ObjectEntitySchema.ObjectEntity, bool>> IsAvaThanksCampaignMemberRule = (obj) => obj.Type == ObjectType.UserDataRealtor;
        public static Expression<Func<aska.core.models.ObjectEntitySchema.ObjectEntity, bool>> IsAvaThanksBonusRule = (obj) => obj.Type == ObjectType.AvaThanksBonus;

        public static Func<aska.core.models.ObjectEntitySchema.ObjectEntity, decimal> AvaThanksBonusCostSelector = (obj) => obj
            .Values.Let(x => x, new List<ObjectParameterValue>())
            .Where(v => v.ParameterKey == ObjectParameterKey.CostTotal)
            .Select(v => (decimal)v.AsTyped(0))
            .FirstOrDefault();

    }
}