using System;
using System.IO;
using kd.domainmodel.Attachment;
using kd.services.attachment.Handlers.Paths;
using NLog;

namespace kd.services.attachment.Store
{
    internal class FileAttachmentStore : IAttachmentStore
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IConstructAttachmentPathHandler _constructAttachmentPathHandler;

        public FileAttachmentStore(IConstructAttachmentPathHandler constructAttachmentPathHandler)
        {
            _constructAttachmentPathHandler = constructAttachmentPathHandler;
        }

        public bool Store(AttachmentEntity attachment, ObjectAttachmentVariant variant, Stream stream)
        {
            if (attachment == null || stream == null) return false;

            var filepath = _constructAttachmentPathHandler.Handle(attachment, variant);
            var folder = Path.GetDirectoryName(filepath);

            Logger.Debug("Store file attachment in variant {1}. File: [{0}/{2}]. Target path: [{3}]", attachment.Id, variant, attachment.OriginalFileName, filepath);
            
            if (!string.IsNullOrEmpty(folder)) Directory.CreateDirectory(folder);

            try
            {
                using (var fs = new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    stream.CopyTo(fs);
                    fs.Flush();
                }
            }
            catch (Exception e)
            {
                Logger.Fatal(e, "Attachment in variant {1}. File: [{0}/{2}]. Target path: [{3}]", attachment.Id, variant, attachment.OriginalFileName, filepath);
                return false;
            }

            return true;
        }
    }
}