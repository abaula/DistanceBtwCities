using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;

namespace DistanceBtwCities.WebApi.Caching
{
    internal class CacheManager : ICacheManager
    {
        private readonly ICacheSettingsManager _cacheSettingsManager;

        public CacheManager(ICacheSettingsManager cacheSettingsManager)
        {
            _cacheSettingsManager = cacheSettingsManager;
        }

        /// <summary>
        /// Ищет кэшированные данные для входящего запроса.
        /// </summary>
        /// <param name="request">Входящий запрос.</param>
        /// <returns>Объект кэширования или null.</returns>
        public CacheDataItem GetCacheDataForRequest(HttpRequestMessage request)
        {
            // пробуем получить данные из кэша.
            var key = _getCacheKeyFromRequest(request);

            var cache = MemoryCache.Default;
            var cacheData = (CacheDataItem) cache.Get(key);

            if (cacheData != null)
            {
                // возвращаем данные из кэша.
                return cacheData;
            }

            // отсутствуют данные в кэше и отсутствуют настройки кэширования.
            return null;
        }

        /// <summary>
        /// Проверяет тэг IfNoneMatch с Etag кэшированных данных.
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <param name="cacheData">Кэшированные данные</param>
        /// <returns>true, если значения IfNoneMatch и Etag совпадают</returns>
        public bool CheckIfCachedDataIsValidForClient(HttpRequestMessage request, CacheDataItem cacheData)
        {
            if (request.Headers.IfNoneMatch.Any())
            {
                foreach (var tagHeader in request.Headers.IfNoneMatch)
                {
                    if (string.Compare(tagHeader.Tag, cacheData.ETag, StringComparison.InvariantCulture) == 0)
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Добавляет заголовки для кэширования на стороне клиента.
        /// </summary>
        /// <param name="response">Ответ</param>
        /// <param name="cachedData">Кэшированные данные</param>
        public void AddResponseHeadersForCachingOnClientSide(HttpResponseMessage response, CacheDataItem cachedData)
        {
            response.Headers.ETag = new EntityTagHeaderValue(cachedData.ETag);

            if (response.Content != null)
                response.Content.Headers.LastModified = cachedData.CreatedAt.ToUniversalTime();

            response.Headers.CacheControl = new CacheControlHeaderValue
            {
                MaxAge = TimeSpan.FromMinutes(cachedData.Settings.MaxAgeMinutes),
                Public = true
            };
        }

        /// <summary>
        /// Проверяет нужно ли кэшировать ответ для запроса.
        /// Если заданы настройки кэширования для запроса, то содержимое ответа сохраняется в кэше.
        /// </summary>
        /// <param name="response">Ответ</param>
        /// <returns>Если ответ был сохранён в кэше, то возвращаем объект кэшированных данных, иначе null</returns>
        public CacheDataItem SaveResponseInCache(HttpResponseMessage response)
        {
            var cachedDataItem = _createCacheItemForRequest(response.RequestMessage);

            if (cachedDataItem != null)
            {
                // создаём ETag
                cachedDataItem.ETag = string.Format("\"{0}\"", Guid.NewGuid().ToString("N"));

                // устанавливаем дату создания кэшированного объекта
                cachedDataItem.CreatedAt = DateTime.Now;

                // устанавливаем содержимое ответа и его тип
                var content = (ObjectContent) response.Content;
                cachedDataItem.Data = content.Value;
                cachedDataItem.DataType = content.ObjectType;

                // создаём ключ для кэша
                var key = _getCacheKeyFromRequest(response.RequestMessage);

                // создаём политику хранения данных
                var policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(cachedDataItem.Settings.MaxAgeMinutes);

                // сохраняем данные в кэше
                var cache = MemoryCache.Default;
                cache.Add(key, cachedDataItem, policy);
            }

            return cachedDataItem;
        }

        private CacheDataItem _createCacheItemForRequest(HttpRequestMessage request)
        {
            // пробуем получить настройки кэширования для запроса.
            var cacheSettings = _cacheSettingsManager.GetCacheSettingsForRequest(request.RequestUri);

            if (cacheSettings != null)
            {
                // настройки кэширования получены, создаём объект кэширования и возвращаем.
                var cacheData = new CacheDataItem {Settings = cacheSettings};

                return cacheData;
            }

            return null;
        }


        private string _getCacheKeyFromRequest(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath;
        }
    }
}