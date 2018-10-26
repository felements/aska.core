using kd.domainmodel.Attachment;

namespace kd.services.attachment.Handlers.Paths
{
    public interface IConstructAttachmentPathHandler
    {
        string Handle(AttachmentEntity attachment, ObjectAttachmentVariant variant);
    }
}