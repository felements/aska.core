using System;
using System.Collections.Generic;
using kd.domainmodel.Attachment;
using kd.services.attachment.Converter.ConversionTaskFactory;
using kd.services.attachment.Handlers.Paths;

namespace kd.services.attachment.Handlers.Variants
{
    public class ImageAttachmentVariantsHandler : IAttachmentVariantsHandler
    {
        private readonly IAttachmentConversionTaskFactory _conversionTaskFactory;
        private readonly IConstructAttachmentPathHandler _constructAttachmentPathHandler;

        public ImageAttachmentVariantsHandler(
            IAttachmentConversionTaskFactory conversionTaskFactory, 
            IConstructAttachmentPathHandler constructAttachmentPathHandler)
        {
            _conversionTaskFactory = conversionTaskFactory;
            _constructAttachmentPathHandler = constructAttachmentPathHandler;
        }

        public void Handle(AttachmentEntity attachment)
        {
            if (attachment.Type != FileAttachmentType.Image) throw new ArgumentOutOfRangeException(nameof(attachment), "Type should be 'image'");

            attachment.Variants = new List<ObjectAttachmentVariant>
            {
                ObjectAttachmentVariant.Original,
                ObjectAttachmentVariant.SmallImage,
                ObjectAttachmentVariant.MediumImage,
                ObjectAttachmentVariant.LargeImage,
                ObjectAttachmentVariant.SquareLargeImage,
                ObjectAttachmentVariant.SquareSmallImage
            };

            // post resize tasks
            var sourcePath = _constructAttachmentPathHandler.Handle(attachment, ObjectAttachmentVariant.Original);

            _conversionTaskFactory.Post(new AttachmentConversionTask(ObjectAttachmentVariant.SmallImage, sourcePath, _constructAttachmentPathHandler.Handle(attachment, ObjectAttachmentVariant.SmallImage)));
            _conversionTaskFactory.Post(new AttachmentConversionTask(ObjectAttachmentVariant.SquareSmallImage, sourcePath, _constructAttachmentPathHandler.Handle(attachment, ObjectAttachmentVariant.SquareSmallImage)));
            _conversionTaskFactory.Post(new AttachmentConversionTask(ObjectAttachmentVariant.MediumImage, sourcePath, _constructAttachmentPathHandler.Handle(attachment, ObjectAttachmentVariant.MediumImage)));
            _conversionTaskFactory.Post(new AttachmentConversionTask(ObjectAttachmentVariant.LargeImage, sourcePath, _constructAttachmentPathHandler.Handle(attachment, ObjectAttachmentVariant.LargeImage)));
            _conversionTaskFactory.Post(new AttachmentConversionTask(ObjectAttachmentVariant.SquareLargeImage, sourcePath, _constructAttachmentPathHandler.Handle(attachment, ObjectAttachmentVariant.SquareLargeImage)));
        }
    }
}