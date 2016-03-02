using System;
using DistanceBtwCities.WebApi.Configuration;

namespace DistanceBtwCities.WebApi.Caching
{
    internal interface ICacheSettingsManager
    {
        CacheSettingsElement GetCacheSettingsForRequest(Uri requestUri);
    }
}