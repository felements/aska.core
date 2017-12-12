using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace aska.core.models.ObjectEntitySchema.Attachments
{
    public partial class ObjectAttachment : IRegularEntity, IEntityChangeTrace
    {
        private const char VariantsSeparator = '|';

        [Obsolete("Only for model binders and EF, don't use it in your code", true)]
        public ObjectAttachment()
        {
            
        }

        public ObjectAttachment(string originalFileName, string title, FileAttachmentType type = FileAttachmentType.Common, ObjectAttachmentVariant[] variants = null)
        {
            Id = Guid.NewGuid();
            Title = title;
            Type = type;
            OriginalFileName = originalFileName;
            Variants = variants ?? new[] {ObjectAttachmentVariant.Original};
            VariantsState = ObjectAttachmentVariantsState.Unknown;
        }

        [Key, Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Attachment display name
        /// </summary>
        public string Title { get; set; }

        public string OriginalFileName { get; set; }

        public DateTime LastModifiedDateUtc { get; set; }

        public string LastModifiedBy { get; set; }

        public FileAttachmentType Type { get; set; }

        public string RawVariants { get; set; }

        public ObjectAttachmentVariantsState VariantsState { get; set; }

        [NotMapped]
        public IEnumerable<ObjectAttachmentVariant> Variants
        {
            get
            {
                if (string.IsNullOrEmpty(RawVariants)) return null;

                var variants = RawVariants
                    .Split(new[] {VariantsSeparator}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x=> (ObjectAttachmentVariant)Enum.Parse(typeof(ObjectAttachmentVariant), x, true));
                return variants;
            }
            set {
                RawVariants = value == null 
                    ? null 
                    : string.Join(VariantsSeparator.ToString(), value.Select(x=>x.ToString("G"))  );
            }
        }

        public Expression<Func<IEntity, bool>> CompareIdExpression()
        {
            return entity => entity.Id.Equals(Id);
        }
    }
}