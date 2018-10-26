using System.Text;
using System.Text.RegularExpressions;
using kd.domainmodel.Attachment;

namespace kd.services.attachment.Handlers.Paths
{
    public class ConstructAttachmentStubWebRelatedPathHandler : IConstructAttachmentPathHandler
    {
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
            
            // {id}
            builder.Append("fallback");
            builder.Append('.');
            // {variant}
            builder.Append(variant.ToString("G"));
            builder.Append(".jpg");
            
            var path = builder.ToString().ToLowerInvariant();
            path = SeparatorsExpression.Replace(path, separator);
            return path;
        }
    }
}