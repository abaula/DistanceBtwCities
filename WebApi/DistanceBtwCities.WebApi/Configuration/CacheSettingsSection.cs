using System.Configuration;

namespace DistanceBtwCities.WebApi.Configuration
{
    public class CacheSettingsSection : ConfigurationSection
    {
        [ConfigurationProperty("cacheSettings", IsRequired = true)]
        public CacheSettingsElementCollection CacheSettings
        {
            get { return base["cacheSettings"] as CacheSettingsElementCollection; }
        }
    }
}