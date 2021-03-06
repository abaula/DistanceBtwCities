﻿using System.Configuration;

namespace DistanceBtwCities.WebApi.Configuration
{
    public class CacheSettingsElement : ConfigurationElement
    {
        [ConfigurationProperty("pattern", IsKey = true, IsRequired = true)]
        public string Pattern
        {
            get { return (string) base["pattern"]; }
            set { base["pattern"] = value; }
        }

        [ConfigurationProperty("maxAgeMinutes", IsKey = true, IsRequired = true)]
        public int MaxAgeMinutes
        {
            get { return (int) base["maxAgeMinutes"]; }
            set { base["maxAgeMinutes"] = value; }
        }
    }
}