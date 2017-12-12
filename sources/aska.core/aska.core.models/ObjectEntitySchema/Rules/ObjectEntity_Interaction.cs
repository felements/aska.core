using System;
using System.Linq.Expressions;
using aska.core.models.ObjectEntitySchema;

// ReSharper disable once CheckNamespace
namespace ask.realty.contracts.models.ObjectEntitySchema
{
    public partial class ObjectEntity
    {
        public static Expression<Func<aska.core.models.ObjectEntitySchema.ObjectEntity, bool>> InteractionChannelObjectRule = (obj) => obj.Type == ObjectType.InteractionChannel;

        public static Expression<Func<aska.core.models.ObjectEntitySchema.ObjectEntity, bool>> InteractionOccasionRule = (obj) => obj.Type == ObjectType.InteractionOccasion 
        || obj.Type == ObjectType.InteractionOccasion_CallbackRequest
        || obj.Type == ObjectType.InteractionOccasion_CallRecord
        || obj.Type == ObjectType.InteractionOccasion_FeedbackForm;
    }
}