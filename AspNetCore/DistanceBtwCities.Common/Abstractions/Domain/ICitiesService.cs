using System.Collections.Generic;
using System.Threading.Tasks;
using DistanceBtwCities.Common.Dtos;

namespace DistanceBtwCities.Common.Abstractions.Domain
{
    /// <summary>
    /// Интерфейс сервиса доступа к данным городов.
    /// </summary>
    public interface ICitiesService
    {
        /// <summary>
        /// Поиск города по запросу - первым символам имени города.
        /// <remarks>Поиск выполняется по указанной подстроке в имени города, начиная с начала города. 
        /// Вот так работает поиск - запрос: "Кра", ответ: "Красавино", "Красновишерск"...</remarks>
        /// </summary>
        /// <param name="query">Поисковый запрос.</param>
        /// <returns>Список найденных городов.</returns>
        Task<IEnumerable<CityInfo>> SearchCity(string query);
    }
}