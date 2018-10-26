using System;
using System.Collections.Generic;
using kd.domainmodel.Attachment;

namespace kd.services.attachment.Models
{
    public class WebObjectAttachment
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string OriginalFileName { get; set; }

        public DateTime LastModifiedDateUtc { get; set; }

        public string LastModifiedBy { get; set; }

        public FileAttachmentType Type { get; set; }

        public IList<KeyValuePair<ObjectAttachmentVariant, string>> Urls { get; set; }
    }
}