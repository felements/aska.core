using System;
using System.IO;
using kd.domainmodel.Attachment;
using kd.services.attachment.Extensions;

namespace kd.services.attachment.Factory
{
    internal class AttachmentFactory  : IAttachmentFactory
    {
        /// <summary>
        /// Create attachment instance with prefilled fields
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public AttachmentEntity Create(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename)) return null;

            filename = filename.ToLowerInvariant();
            var title = FormatTitle(filename);
            var type = AttachmentExtensions.GetAttachmentType(filename);
            
            var attachment = new AttachmentEntity(filename, title, type)
            {
                LastModifiedDateUtc = DateTime.UtcNow,
                MimeType = AttachmentExtensions.GetattachmentMimeType(filename)
            };
            return attachment;
        }

        private static string FormatTitle(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename)) return "-";

            return Path.GetFileNameWithoutExtension(filename).Replace('_', ' ');
        }

        public static string CreateStorePath(Guid id, string storePath, string filename)
        {
            var storeFileName = id.ToString("D") + Path.GetExtension(filename ?? string.Empty);
            var path = Environment.CurrentDirectory + storePath.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar) + storeFileName;

            return path;
        }
    }
}