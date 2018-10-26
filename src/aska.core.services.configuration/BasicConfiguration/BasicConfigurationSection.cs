using System;
using System.Configuration;

namespace ferriswheel.services.config.BasicConfiguration
{
    public class BasicConfigurationSection : ConfigurationSection
    {
        private static readonly Lazy<BasicConfigurationSection> Configuration = new Lazy<BasicConfigurationSection>(() =>
            (BasicConfigurationSection)ConfigurationManager.GetSection(Constants.SectionName));

        public static BasicConfigurationSection Instance
        {
            get { return Configuration.Value; }
        }

        [ConfigurationProperty(Constants.ParameterElement.CollectionName, IsRequired = true)]
        [ConfigurationCollection(typeof(ParameterElement), AddItemName = Constants.ParameterElement.ElementName)]
        public GenericConfigurationElementCollection<ParameterElement> Parameters
        {
            get { return (GenericConfigurationElementCollection<ParameterElement>)this[Constants.ParameterElement.CollectionName]; }
        }
    }
}