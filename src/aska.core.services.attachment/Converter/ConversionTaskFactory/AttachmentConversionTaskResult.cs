using System;

namespace kd.services.attachment.Converter.ConversionTaskFactory
{
    public class AttachmentConversionTaskResult
    {
        [Obsolete("Only for model binders and EF, don't use it in your code", true)]
        public AttachmentConversionTaskResult() {}

        public AttachmentConversionTaskResult(bool succeed, string log = null)
        {
            Success = succeed;
            OperationLog = log;
        }

        public bool Success { get; set; }
        public string OperationLog { get; set; }
    }
}