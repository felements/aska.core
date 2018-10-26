using System;
using System.Collections.Generic;
using kd.domainmodel.Attachment;

namespace kd.services.attachment.Handlers.Variants
{
    public class CommonAttachmentVariantsHandler : IAttachmentVariantsHandler
    {
        public void Handle(AttachmentEntity attachment)
        {
            if (attachment.Type != FileAttachmentType.Common) throw new ArgumentOutOfRangeException(nameof(attachment), "Type should be 'common'");

            attachment.Variants = new List<ObjectAttachmentVariant>
            {
                ObjectAttachmentVariant.Original
            };
        }
    }
}