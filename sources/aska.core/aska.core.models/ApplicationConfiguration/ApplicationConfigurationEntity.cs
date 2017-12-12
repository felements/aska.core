using System;
using System.Linq.Expressions;
using aska.core.models.MetaValueTypes;

namespace aska.core.models.ApplicationConfiguration
{
    public class ApplicationConfigurationEntity : IRegularEntity, IEntityChangeTrace
    {
        [Obsolete("Only for model binders and EF, don't use it in your code", true)]
        public ApplicationConfigurationEntity() {}


        public ApplicationConfigurationEntity(string configurationKey, ISerializedMetaValue value, string defaultRawValue)
        {
            Id = Guid.NewGuid();
            ConfigurationKey = configurationKey;
            RawValue = value.RawValue;
            ValueType = value.ValueType;
            DefaultRawValue = defaultRawValue;
        }

        [Key, Required]
        public Guid Id { get; set; }

        public string ConfigurationKey { get; set; }

        /// <summary>
        /// The value that is set by the user
        /// </summary>
        public string RawValue { get; set; }

        public MetaValueType ValueType { get; set; }

        /// <summary>
        /// The value that is set by the deployment script and used in case of empty RawValue
        /// </summary>
        public string DefaultRawValue { get; set; }


        public Expression<Func<IEntity, bool>> CompareIdExpression()
        {
            return entity => entity.Id.Equals(Id);
        }

        public DateTime LastModifiedDateUtc { get; set; }
        public string LastModifiedBy { get; set; }
    }
}