using System;
using System.IO;
using System.Runtime.Caching;
using System.Text;
using System.Web;
using DistanceBtwCities.WebApi.Properties;

namespace DistanceBtwCities.WebApi.Models
{
    public class CachedPageProvider
    {
        private const string HtmlPageContentKeyPrefix = "HtmlPageContent@";
        private static readonly object CacheLock = new object();

        public static string GetHtmlPageContent(string path)
        {
            var key = HtmlPageContentKeyPrefix + path;
            var cache = MemoryCache.Default;
            var cachedPage = (string)cache.Get(key);

            if (cachedPage != null)
                return cachedPage;

            lock (CacheLock)
            {
                cachedPage = (string)cache.Get(key);

                if (cachedPage != null)
                    return cachedPage;

                var htmlPath = HttpContext.Current.Server.MapPath(path);
                cachedPage = File.ReadAllText(htmlPath, Encoding.UTF8);
                // создаём политику хранения данных
                var policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(Settings.Default.HtmlPageCacheAge);
                // сохраняем данные в кэше
                cache.Add(key, cachedPage, policy);

                return cachedPage;
            }
        }
    }
}