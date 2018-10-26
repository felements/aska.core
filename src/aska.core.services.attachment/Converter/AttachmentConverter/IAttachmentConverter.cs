using kd.domainmodel.Attachment;
using kd.services.attachment.Converter.ConversionTaskFactory;

namespace kd.services.attachment.Converter.AttachmentConverter
{
    public interface IAttachmentConverter
    {
        AttachmentConversionTaskResult Proceed(ObjectAttachmentVariant variant, string sourceFile, string targetFile);
    }
}