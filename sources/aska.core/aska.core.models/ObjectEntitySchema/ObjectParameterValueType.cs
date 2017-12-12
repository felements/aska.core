namespace aska.core.models.ObjectEntitySchema
{
    public enum ObjectParameterValueType
    {
        Unknown = 0,

        /// <summary>
        /// Simple text field
        /// </summary>
        Text = 1,

        /// <summary>
        /// Simple text field with limitation to enter only numbers
        /// </summary>
        Number = 2,

        Date = 3,
        DateTime = 4,

        /// <summary>
        /// Link to another object. Should be rendered as <select> with single or multiple selections. 
        /// Set of available values are provided by the <see cref="IObjectParameterValueVariantsProvider"/> implementations
        /// </summary>
        Reference = 5,

        /// <summary>
        /// Multiline text field with support of formatting (Markdown is preferred)
        /// </summary>
        FormattedText = 6,

        /// <summary>
        /// Attached file (image, document)...all the user's stuff that stored in FS
        /// Handled by <see cref="IAttachmentService"/>
        /// </summary>
        Attachment = 7,

        /// <summary>
        /// simple check-box
        /// </summary>
        Boolean = 8,

        Static = 9,
    }
}