using System.Net.Http;

namespace DistanceBtwCities.WebApi.Caching
{
    public interface ICacheManager
    {
        void AddResponseHeadersForCachingOnClientSide(HttpResponseMessage response, CacheDataItem cachedData);
        bool CheckIfCachedDataIsValidForClient(HttpRequestMessage request, CacheDataItem cacheData);
        CacheDataItem GetCacheDataForRequest(HttpRequestMessage request);
        CacheDataItem SaveResponseInCache(HttpResponseMessage response);
    }
}