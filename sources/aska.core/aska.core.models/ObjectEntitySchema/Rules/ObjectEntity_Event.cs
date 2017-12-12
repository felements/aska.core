using System;
using System.Linq;
using System.Linq.Expressions;
using aska.core.models.ObjectEntitySchema;

// ReSharper disable once CheckNamespace
namespace ask.realty.contracts.models.ObjectEntitySchema
{
    public partial class ObjectEntity
    {
        public static Expression<Func<aska.core.models.ObjectEntitySchema.ObjectEntity, bool>> EventObjectRule = (obj) => obj.Type == ObjectType.EventNewsText || obj.Type == ObjectType.EventPromo || obj.Type == ObjectType.EventNewsVideo;
        public static Expression<Func<aska.core.models.ObjectEntitySchema.ObjectEntity, bool>> PhotoObjectRule = (obj) => obj.Type == ObjectType.PhotoFeed || obj.Type == ObjectType.PhotoReport;
        public static Expression<Func<aska.core.models.ObjectEntitySchema.ObjectEntity, bool>> PhotoreportObjectRule = (obj) => obj.Type == ObjectType.PhotoReport;
        public static Expression<Func<aska.core.models.ObjectEntitySchema.ObjectEntity, bool>> PhotoFeedObjectRule = (obj) => obj.Type == ObjectType.PhotoFeed;

        public static Expression<Func<aska.core.models.ObjectEntitySchema.ObjectEntity, string>> OccasionDateSelector = (obj) => obj.Values.Where(v => v.ParameterKey == ObjectParameterKey.OccasionDate).Select(v => v.RawValue).FirstOrDefault();
    }
}