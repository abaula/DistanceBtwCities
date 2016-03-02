using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using DistanceBtwCities.WebApi.Configuration;

namespace DistanceBtwCities.WebApi.Caching
{
    internal class CacheSettingsManager : ICacheSettingsManager
    {
        /// <summary>
        ///     Настройки кэширования в структуре обеспечивающей быстрый поиск настроект для входящего запроса.
        /// </summary>
        private ILookup<int, KeyValuePair<CachePathSegments, CacheSettingsElement>> _cacheSettingsLookup;

        /// <summary>
        ///     Объект реализующий сравнение входящего запроса с настройками кэширования.
        ///     Используется с целью определить существуют ли настройки кэширования для входящего запроса.
        /// </summary>
        private readonly CacheSegmentsComparer _сacheSegmentsComparer;

        /// <summary>
        ///     Конструктор по умолчанию скрыт, чтобы реализовать патерн Singleton.
        /// </summary>
        public CacheSettingsManager(CacheSegmentsComparer сacheSegmentsComparer)
        {
            _сacheSegmentsComparer = сacheSegmentsComparer;

            _init();
        }

        /// <summary>
        ///     Инициализирует объекты настроек кэширования для быстрого поиска настроек кэширования для входящего запроса.
        /// </summary>
        private void _init()
        {
            var cacheSettings = (CacheSettingsSection) ConfigurationManager.GetSection("cacheSettingsSection");
            var lookupItems = new List<KeyValuePair<CachePathSegments, CacheSettingsElement>>();

            foreach (var cahceSettingElement in cacheSettings.CacheSettings.Cast<CacheSettingsElement>())
            {
                var segments = new CachePathSegments(cahceSettingElement.Pattern);
                var lookupItem = new KeyValuePair<CachePathSegments, CacheSettingsElement>(segments, cahceSettingElement);
                lookupItems.Add(lookupItem);
            }

            _cacheSettingsLookup = lookupItems.ToLookup(i => i.Key.Segments.Count);
        }

        /// <summary>
        ///     Получает настройки кэширования из конфигурации для входящего запроса.
        /// </summary>
        /// <param name="requestUri">Uri входящего запроса.</param>
        /// <returns>Настройки кэширования или null.</returns>
        public CacheSettingsElement GetCacheSettingsForRequest(Uri requestUri)
        {
            var requestSegmentsCount = requestUri.Segments.Count();

            // Получаем все настройки с таким-же числом сегментов, как и у входящего запроса.
            var cacheSettingsEnumerable = _cacheSettingsLookup[requestSegmentsCount];

            foreach (var pair in cacheSettingsEnumerable)
            {
                var cachePathSegments = pair.Key;

                if (_сacheSegmentsComparer.AreEqual(cachePathSegments.Segments, requestUri.Segments))
                    return pair.Value;
            }

            return null;
        }
    }
}