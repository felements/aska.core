using System.Configuration;

namespace ferriswheel.services.config.BasicConfiguration
{
    public class ParameterElement : ConfigurationElement
    {
        [ConfigurationProperty(Constants.ParameterElement.Key, IsRequired = true, IsKey = true)]
        public string Key
        {
            get { return (string)base[Constants.ParameterElement.Key]; }
            set { base[Constants.ParameterElement.Key] = value; }
        }

        [ConfigurationProperty(Constants.ParameterElement.Value, IsRequired = false, DefaultValue = "")]
        public string Value
        {
            get { return (string)base[Constants.ParameterElement.Value]; }
            set { base[Constants.ParameterElement.Value] = value; }
        }
    }
}