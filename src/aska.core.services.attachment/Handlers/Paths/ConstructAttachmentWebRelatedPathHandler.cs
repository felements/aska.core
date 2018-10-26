using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using kd.domainmodel.Attachment;

namespace kd.services.attachment.Handlers.Paths
{
    public class ConstructAttachmentWebRelatedPathHandler : IConstructAttachmentPathHandler
    {
        private const byte SubpathDepth = 3; //TODO: move to configuration
        private static readonly Regex SeparatorsExpression = new Regex(@"[\\/]+", 
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// Construct path for accessing via web
        /// </summary>
        /// <remarks>
        /// format:  attachments / {sub_path} / {id}.{variant}.{ext}</remarks>
        /// <param name="attachment"></param>
        /// <param name="variant"></param>
        /// <returns></returns>
        public string Handle(AttachmentEntity attachment, ObjectAttachmentVariant variant)
        {
            var separator = "/";

            var builder = new StringBuilder();
            builder.Append(separator);
            builder.Append("data");
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