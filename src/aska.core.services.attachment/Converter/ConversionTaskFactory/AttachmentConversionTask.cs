using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using kd.domainmodel;
using kd.domainmodel.Attachment;
using kd.domainmodel.Entity;

namespace kd.services.attachment.Converter.ConversionTaskFactory
{
    public class AttachmentConversionTask : IEntity
    {
        public static Expression<Func<AttachmentConversionTask, bool>> PendingRule = (obj) => !obj.Completed || (obj.Completed && !(obj.Succeed ?? false)  );
        public static Expression<Func<AttachmentConversionTask, bool>> EqualsRule = (obj) => !obj.Completed;


        [Obsolete("Only for model binders and EF, don't use it in your code", true)]
        public AttachmentConversionTask() {}

        public AttachmentConversionTask(ObjectAttachmentVariant variant, string sourcePath, string resultPath)
        {
            Id = Guid.NewGuid();
            Completed = false;
            OperationLog = null;
            CreationDateTimeUtc = DateTime.UtcNow;

            ResultVariant = variant;
            ResultFilePath = Args.EnsureNotEmpty(resultPath, nameof(resultPath));
            SourceFilePath = Args.EnsureNotEmpty(sourcePath, nameof(sourcePath));
        }

        [Required]
        public string SourceFilePath { get; set; }

        [Required]
        public string ResultFilePath { get; set; }

        [Required]
        public ObjectAttachmentVariant ResultVariant { get; set; }

        
        //todo: [Index]
        public bool Completed { get; set; }
        public bool? Succeed { get; set; }

        public DateTime CreationDateTimeUtc { get; set; }
        public DateTime? ExecutionStartDateTimeUtc { get; set; }
        public DateTime? ExecutionEndDateTimeUtc { get; set; }
        public string OperationLog { get; set; }

        [Key]
        [Required]
        public Guid Id { get; set; }

        public string GetId()
        {
            return Id.ToString("D");
        }
    }
}