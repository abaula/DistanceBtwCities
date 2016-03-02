using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DistanceBtwCities.WebApi.Caching;

namespace DistanceBtwCities.WebApi.Handlers
{
    public class MessageHandlerForCaching : DelegatingHandler
    {
        private readonly ICacheManager _cacheManager;


        public MessageHandlerForCaching(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // 1. пробуем найти данные в кэше
            if (request.Method == HttpMethod.Get)
            {
                var response = _tryGetResponseFromCache(request);

                // ... если нашли данные в кэше, то возврашаем их клиенту
                if (response != null)
                    return response;
            }

            // 2. пропускаем запрос далее по цепочке.
            var responseMessage = await base.SendAsync(request, cancellationToken);

            // 3. при необходимости помещаем данные в кэш и настраиваем заголовки ответа
            if (request.Method == HttpMethod.Get)
                _trySaveResponseInCache(responseMessage);


            return responseMessage;
        }

        private HttpResponseMessage _tryGetResponseFromCache(HttpRequestMessage request)
        {
            // пробуем получить данные из кэша 
            var cachedData = _cacheManager.GetCacheDataForRequest(request);

            // если данные есть в кэше, то возвращаем результат из кэша.
            if (cachedData != null)
            {
                HttpResponseMessage response;

                // проверяем отправил ли клиент заголовок If-None-Match.
                var isMatchForClient = _cacheManager.CheckIfCachedDataIsValidForClient(request, cachedData);

                if (isMatchForClient)
                {
                    // Случай 1. Клиент имеет данные в кэше и просит проверить их актуальность (клиент отправил заголовок If-None-Match).
                    response = new HttpResponseMessage(HttpStatusCode.NotModified);
                }
                else
                {
                    // Случай 2. Клиент не имеет данных в кэше или они не актуальны.
                    // ... создаём ответ
                    response = request.CreateResponse(HttpStatusCode.OK, cachedData.Data);
                    // ... меняем содержимое ответа полученное из кэша, на явно указанный тип, чтобы использовать сгенетированные VS конвертеры
                    var newContent = new ObjectContent(cachedData.DataType, cachedData.Data,
                        ((ObjectContent) response.Content).Formatter);
                    response.Content = newContent;
                }

                // настраиваем заголовки ответа
                _cacheManager.AddResponseHeadersForCachingOnClientSide(response, cachedData);
                // возвращаем кэшированные данные
                return response;
            }

            // в кэше отсутствуют данные для запроса. Возвращаем null.
            return null;
        }

        private void _trySaveResponseInCache(HttpResponseMessage response)
        {
            // пробуем сохранить данные в кэше 
            var cahcedData = _cacheManager.SaveResponseInCache(response);

            // настраиваем заголовки ответа для кэширования на строне клиента.
            if (cahcedData != null)
                _cacheManager.AddResponseHeadersForCachingOnClientSide(response, cahcedData);
        }
    }
}