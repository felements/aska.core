namespace aska.core.models.ObjectEntitySchema.Attachments
{
    public enum ObjectAttachmentVariantsState
    {
        Unknown = 0,

        /// <summary>
        /// Variants re-generation is required
        /// </summary>
        Failsafe = 1,

        /// <summary>
        /// Regeneration is in progress
        /// </summary>
        Processing = 2,
        
        /// <summary>
        /// Original file is missing
        /// </summary>
        Missing = 3,

        /// <summary>
        /// Everything is ok
        /// </summary>
        Ok = 10,
    }
}