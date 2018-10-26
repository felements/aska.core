using kd.domainmodel.Attachment;

namespace kd.services.attachment.Handlers.Variants
{
    public interface IAttachmentVariantsHandler
    {
        void Handle(AttachmentEntity attachment);
    }
}