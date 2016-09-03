using System.Net.Http;

namespace DistanceBtwCities.WebApi.Caching
{
    /// <summary>
    /// Интерфейс для объекта управления кэшированием.
    /// </summary>
    public interface ICacheManager
    {
        /// <summary>
        /// Добавляет заголовки для кэширования на стороне клиента.
        /// </summary>
        /// <param name="response">Ответ</param>
        /// <param name="cachedData">Кэшированные данные</param>
        void AddResponseHeadersForCachingOnClientSide(HttpResponseMessage response, CacheDataItem cachedData);

        /// <summary>
        /// Проверяет тэг IfNoneMatch с Etag кэшированных данных.
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <param name="cacheData">Кэшированные данные</param>
        /// <returns>true, если значения IfNoneMatch и Etag совпадают</returns>
        bool CheckIfCachedDataIsValidForClient(HttpRequestMessage request, CacheDataItem cacheData);

        /// <summary>
        /// Ищет кэшированные данные для входящего запроса.
        /// </summary>
        /// <param name="request">Входящий запрос.</param>
        /// <returns>Объект кэширования или null.</returns>
        CacheDataItem GetCacheDataForRequest(HttpRequestMessage request);

        /// <summary>
        /// Проверяет нужно ли кэшировать ответ для запроса.
        /// Если заданы настройки кэширования для запроса, то содержимое ответа сохраняется в кэше.
        /// </summary>
        /// <param name="response">Ответ</param>
        /// <returns>Если ответ был сохранён в кэше, то возвращаем объект кэшированных данных, иначе null</returns>
        CacheDataItem SaveResponseInCache(HttpResponseMessage response);
    }
}