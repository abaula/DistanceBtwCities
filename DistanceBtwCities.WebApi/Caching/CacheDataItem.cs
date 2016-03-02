using System;
using DistanceBtwCities.WebApi.Configuration;

namespace DistanceBtwCities.WebApi.Caching
{
    public class CacheDataItem
    {
        public object Data { get; set; }
        public Type DataType { get; set; }
        public DateTime CreatedAt { get; set; }
        public CacheSettingsElement Settings { get; set; }
        public string ETag { get; set; }
    }
}