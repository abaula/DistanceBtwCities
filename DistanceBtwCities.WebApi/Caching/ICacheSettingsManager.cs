using System;
using DistanceBtwCities.WebApi.Configuration;

namespace DistanceBtwCities.WebApi.Caching
{
    /// <summary>
    /// Интерфейс объекта управляющего настройками кэширования.
    /// </summary>
    internal interface ICacheSettingsManager
    {
        /// <summary>
        ///     Получает настройки кэширования из конфигурации для входящего запроса.
        /// </summary>
        /// <param name="requestUri">Uri входящего запроса.</param>
        /// <returns>Настройки кэширования или null.</returns>
        CacheSettingsElement GetCacheSettingsForRequest(Uri requestUri);
    }
}