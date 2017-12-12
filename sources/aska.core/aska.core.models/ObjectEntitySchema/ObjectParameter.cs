using System;
using System.Linq.Expressions;

namespace aska.core.models.ObjectEntitySchema
{
    /// <summary>
    ///     Describes available parameters for concrete ObjectType
    /// </summary>
    public partial class ObjectParameter : IRegularEntity
    {
        private ObjectParameterKey _parameterKey;
        private ObjectParameterValueType _parameterValueType;
        private ObjectType _type;

        [Obsolete("Only for model binders and EF, don't use it in your code", true)]
        internal ObjectParameter()
        {
        }

        public ObjectParameter(
            ObjectType type, 
            ObjectParameterKey parameterKey,
            ObjectParameterValueType parameterValueType,
            ObjectParameterCategory category = ObjectParameterCategory.Common,
            int order = 10000,
            string title = "",
            string description = "",
            bool allowMultipleValues = false,
            bool builtIn = false)
        {
            Id = Guid.NewGuid();
            ObjectType = type;
            ParameterKey = parameterKey;
            ParameterValueType = parameterValueType;
            Title = title;
            Description = description;
            Category = category;
            Order = order;
            AllowMultipleValues = allowMultipleValues;
            BuiltIn = builtIn;
        }

        [Key, Required]
        public Guid Id { get; set; }

        /// <summary>
        ///     Parameter title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     Parameter description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Object Entity type
        /// </summary>
        [Index]
        public ObjectType ObjectType
        {
            get { return _type; }
            set
            {
                if (value <= ObjectType.Unknown) throw new ArgumentOutOfRangeException(nameof(Type));
                _type = value;
            }
        }

        /// <summary>
        ///     Parameter
        /// </summary>
        public ObjectParameterKey ParameterKey
        {
            get { return _parameterKey; }
            set
            {
                if (value <= ObjectParameterKey.Unknown) throw new ArgumentOutOfRangeException(nameof(ParameterKey));
                _parameterKey = value;
            }
        }

        public ObjectParameterValueType ParameterValueType
        {
            get { return _parameterValueType; }
            set
            {
                if (value <= ObjectParameterValueType.Unknown) throw new ArgumentOutOfRangeException(nameof(ParameterValueType));
                _parameterValueType = value;
            }
        }

        /// <summary>
        /// Parameter category
        /// </summary>
        public ObjectParameterCategory Category { get; set; }

        /// <summary>
        /// parameter's display order
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// allow to attach multiple ParameterValue instances to the Entity
        /// </summary>
        public bool AllowMultipleValues { get; set; }

        /// <summary>
        /// Mark the parameter as non-editable in forms customization module. 
        /// This parameters are overwritten after each DB deployment.
        /// </summary>
        public bool BuiltIn { get; set; }

        public Expression<Func<IEntity, bool>> CompareIdExpression()
        {
            return entity => entity.Id.Equals(Id);
        }
    }
}