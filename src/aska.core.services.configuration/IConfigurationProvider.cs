using ferriswheel.datamodel.Content;

namespace ferriswheel.services.config
{
    public interface IConfigurationProvider
    {
        ConfigValueEntity Get(string key);
        ConfigValueEntity[] Get();
        void Set(ConfigValueEntity value);
        ConfigValueEntity Set(string key, string value, ConfigValueType valueType = ConfigValueType.PlainString);
        void Reload(ConfigurationProvider enabledProviders = ConfigurationProvider.All);

        int GetInt(string key);
        bool GetBool(string key);
        string GetStringOrDefault(string key, string defaultValue);
    }
}