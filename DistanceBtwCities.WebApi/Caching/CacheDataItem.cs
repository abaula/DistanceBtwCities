using System;
using DistanceBtwCities.WebApi.Configuration;

namespace DistanceBtwCities.WebApi.Caching
{
    /// <summary>
    /// Объект хранения в кэше.
    /// </summary>
    public class CacheDataItem
    {
        /// <summary>
        /// Хранимый объект.
        /// </summary>
        public object Data { get; set; }
        
        /// <summary>
        /// Тип хранимого объекта.
        /// </summary>
        public Type DataType { get; set; }
        
        /// <summary>
        /// Дата и время помещения объекта в кэш.
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// Настройки кэширования объекта.
        /// </summary>
        public CacheSettingsElement Settings { get; set; }

        /// <summary>
        /// ETag кэшированого объекта.
        /// </summary>
        public string ETag { get; set; }
    }
}