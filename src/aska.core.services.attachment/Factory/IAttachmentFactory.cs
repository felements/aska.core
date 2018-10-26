using kd.domainmodel.Attachment;

namespace kd.services.attachment.Factory
{
    public interface IAttachmentFactory
    {
        AttachmentEntity Create(string filename);
    }
}