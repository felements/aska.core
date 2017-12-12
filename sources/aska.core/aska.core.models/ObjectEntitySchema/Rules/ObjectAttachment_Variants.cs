using System;
using System.Linq.Expressions;
using aska.core.models.ObjectEntitySchema.Attachments;

// ReSharper disable once CheckNamespace
namespace ask.realty.contracts.models.ObjectEntitySchema.Attachments
{
    public partial class ObjectAttachment
    {
        public static Expression<Func<ObjectAttachment, bool>> VariantsRegenerationPendingRule = (obj) => obj.VariantsState == ObjectAttachmentVariantsState.Failsafe;
    }
}