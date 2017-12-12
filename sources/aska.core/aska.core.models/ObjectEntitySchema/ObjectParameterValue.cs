using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace aska.core.models.ObjectEntitySchema
{
    [DebuggerDisplay("Value #{ParameterKey}: {RawValue}")]
    public partial class ObjectParameterValue : IRegularEntity, IEntityChangeTrace
    {
        private Guid _entityId;
        private ObjectParameterKey _parameterKey;

        [Obsolete("Only for model binders and EF, don't use it in your code", true)]
        internal ObjectParameterValue()
        {
        }

        public ObjectParameterValue(ObjectParameterKey parameterKey, Guid objectEntityId, string rawValue = null)
        {
            Id = Guid.NewGuid();
            ParameterKey = parameterKey;
            ObjectEntityId = objectEntityId;
            RawValue = rawValue;
            LastModifiedDateUtc = DateTime.UtcNow;
        }

        [Key, Required]
        public Guid Id { get; set; }

        public string RawValue { get; set; }

        [Required]
        [Index]
        public ObjectParameterKey ParameterKey
        {
            get { return _parameterKey; }
            set
            {
                if (value <= ObjectParameterKey.Unknown) throw new ArgumentOutOfRangeException(nameof(ParameterKey));
                _parameterKey = value;
            }
        }

        [Required]
        [Index]
        public Guid ObjectEntityId
        {
            get { return _entityId; }
            set
            {
                if (value == Guid.Empty) throw new ArgumentOutOfRangeException(nameof(ObjectEntityId));
                _entityId = value;
            }
        }

        public DateTime LastModifiedDateUtc { get; set; }

        public string LastModifiedBy { get; set; }

        public Expression<Func<IEntity, bool>> CompareIdExpression()
        {
            return entity => entity.Id.Equals(Id);
        }
    }
}