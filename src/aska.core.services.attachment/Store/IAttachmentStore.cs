using System.IO;
using kd.domainmodel.Attachment;

namespace kd.services.attachment.Store
{
    public interface IAttachmentStore
    {
        bool Store(AttachmentEntity attachment, ObjectAttachmentVariant variant, Stream stream);
    }
}