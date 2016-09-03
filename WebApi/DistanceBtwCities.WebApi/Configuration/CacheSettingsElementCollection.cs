using System.Configuration;

namespace DistanceBtwCities.WebApi.Configuration
{
    [ConfigurationCollection(typeof (CacheSettingsElement), AddItemName = "cacheSettingsElement")]
    public class CacheSettingsElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new CacheSettingsElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var cacheSettingsElement = element as CacheSettingsElement;

            if (cacheSettingsElement != null)
                return cacheSettingsElement.Pattern;

            return null;
        }
    }
}