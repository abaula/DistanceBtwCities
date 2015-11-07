using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace DistanceBtwCities.WebApi.Configuration
{
    public class CacheSettingsSection : ConfigurationSection 
    {
        [ConfigurationProperty("cacheSettings", IsRequired = true)]
        public CacheSettingsElementCollection CacheSettings
        {
            get
            {
                return base["cacheSettings"] as CacheSettingsElementCollection;
            }
        }
    }
}