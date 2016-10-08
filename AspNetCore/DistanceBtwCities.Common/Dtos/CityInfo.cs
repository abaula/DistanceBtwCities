
namespace DistanceBtwCities.Common.Dtos
{
    /// <summary>
    /// Информация о городе. 
    /// Содержит данные КЛАДР и географические координаты.
    /// </summary>
    /// <see cref="http://www.gnivc.ru/inf_provision/classifiers_reference/kladr/"/>
    public class CityInfo
    {
        /// <summary>
        /// Идентификатор города.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Географическая широта города - проходит приблизительно через центр города.
        /// </summary>
        public decimal Latitude { get; set; }

        /// <summary>
        /// Географическая долгота города - проходит приблизительно через центр города.
        /// </summary>
        public decimal Longitude { get; set; }

        /// <summary>
        /// Название города по КЛАДР.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Район по КЛАДР.
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// Регион города по КЛАДР.
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Суффикс населённого пункта по КЛАДР.
        /// </summary>
        public string Suffix { get; set; }

        /// <summary>
        /// Код КЛАДР.
        /// </summary>
        public string CladrCode { get; set; }

        /// <summary>
        /// Почтовый код.
        /// </summary>
        public string PostCode { get; set; }

        /// <summary>
        /// Полное составное имя города.
        /// </summary>
        public string FullName { get; set; }
    }
}