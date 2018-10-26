using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using kd.domainmodel.Attachment;
using kd.misc;
using kd.misc.Constants;

namespace kd.services.attachment.Handlers.Paths
{
    public class ConstructAttachmentFilesystemRelatedPathHandler : IConstructAttachmentPathHandler
    {
        private const byte SubpathDepth = 3;
        private static readonly Regex SeparatorsExpression = new Regex(@"[\\/]+", 
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// Construct filepath for storing in app folder
        /// </summary>
        /// <remarks>
        /// format: {working_dir} / {Config_Attachments_StoragePath} / {sub_path} / {id}.{variant}.{ext}</remarks>
        /// <param name="attachment"></param>
        /// <param name="variant"></param>
        /// <returns></returns>
        public string Handle(AttachmentEntity attachment, ObjectAttachmentVariant variant)
        {
            var separator = Path.DirectorySeparatorChar.ToString();

            var builder = new StringBuilder();
            // {working_dir}
            builder.Append(AssemblyExtensions.AssemblyDirectory);
            builder.Append(separator);
            // {Config_Attachments_StoragePath}
            builder.Append(Configuration.AttachmentsStoragePath );
            builder.Append(separator);
            // {sub_path}
            var id = attachment.Id.ToString("N");
            for (byte i = 0; i < Math.Min(SubpathDepth, id.Length); i++)
            {
                builder.Append(id[i]);
                builder.Append(separator);
            }
            // {id}
            builder.Append(id);
            builder.Append('.');
            // {variant}
            builder.Append(variant.ToString("G"));
            builder.Append(Path.GetExtension(attachment.OriginalFileName));
            
            var path = builder.ToString().ToLowerInvariant();
            path = SeparatorsExpression.Replace(path, separator);
            return path;
        }
    }
}