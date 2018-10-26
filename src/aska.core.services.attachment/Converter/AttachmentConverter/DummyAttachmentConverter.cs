using kd.domainmodel.Attachment;
using kd.services.attachment.Converter.ConversionTaskFactory;

namespace kd.services.attachment.Converter.AttachmentConverter
{
    public class DummyAttachmentConverter : IAttachmentConverter
    {
        public AttachmentConversionTaskResult Proceed(ObjectAttachmentVariant variant, string sourceFile, string targetFile)
        {
            return new AttachmentConversionTaskResult(true, "No actions were performed.");
        }
    }
}