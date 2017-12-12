using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;

namespace aska.core.models.ObjectEntitySchema
{
    /// <summary>
    ///     Base entity for all public objects (realty, events, etc)
    /// </summary>
    [DebuggerDisplay("Entity #{Type}: {Title}")]
    public partial class ObjectEntity : IRegularEntity, IEntityFakeDeleted, IEntityChangeTrace
    {
        private ObjectType _type;

        [Obsolete("Only for model binders and EF, don't use it in your code", true)]
        internal ObjectEntity()
        {
        }

        public ObjectEntity(ObjectType type, string title = "")
        {
            Id = Guid.NewGuid();
            Type = type;
            Title = title;
            LastModifiedDateUtc = DateTime.UtcNow;
            if (Values == null) Values = new List<ObjectParameterValue>();
        }

        [Key, Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Internal entity title (optional)
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Object type
        /// </summary>
        [Required]
        [Index]
        public ObjectType Type
        {
            get { return _type; }
            set
            {
                if (value <= ObjectType.Unknown) throw new ArgumentOutOfRangeException(nameof(Type));
                _type = value;
            }
        }

        public bool IsDeleted { get; set; }

        public DateTime LastModifiedDateUtc { get; set; }

        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Project parameters values
        /// </summary>
        public virtual ICollection<ObjectParameterValue> Values { get; set; }

        public Expression<Func<string>> GetIdExpression()
        {
            return () => Id.ToString("D");
        }

        public Expression<Func<IEntity, bool>> CompareIdExpression()
        {
            return entity => entity.Id.Equals(Id);
        }
    }
}