using System;
using System.Collections.Generic;
using kd.domainmodel.Attachment;

namespace kd.services.attachment.Handlers.Variants
{
    public class DocumentAttachmentVariantsHandler : IAttachmentVariantsHandler
    {
        public void Handle(AttachmentEntity attachment)
        {
            if (attachment.Type != FileAttachmentType.Document) throw new ArgumentOutOfRangeException(nameof(attachment), "Type should be 'document'");

            attachment.Variants = new List<ObjectAttachmentVariant>
            {
                ObjectAttachmentVariant.Original
            };
        }
    }
}