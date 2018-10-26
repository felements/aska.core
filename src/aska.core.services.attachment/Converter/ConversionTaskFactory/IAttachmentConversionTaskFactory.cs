using System;

namespace kd.services.attachment.Converter.ConversionTaskFactory
{
    public interface IAttachmentConversionTaskFactory
    {
        void Post(AttachmentConversionTask task);
        void Rerun(Guid taskId);
    }
}