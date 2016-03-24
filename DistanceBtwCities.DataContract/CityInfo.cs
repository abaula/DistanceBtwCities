using System.Runtime.Serialization;

namespace DistanceBtwCities.DataContract
{
    /// <summary>
    /// Информация о городе. 
    /// Содержит данные КЛАДР и географические координаты.
    /// </summary>
    /// <see cref="http://www.gnivc.ru/inf_provision/classifiers_reference/kladr/"/>
    [DataContract]
    public class CityInfo
    {
        /// <summary>
        /// Идентификатор города.
        /// </summary>
        [DataMember]
        public long Id { get; set; }

        /// <summary>
        /// Географическая широта города - проходит приблизительно через центр города.
        /// </summary>
        [DataMember]
        public double Latitude { get; set; }

        /// <summary>
        /// Географическая долгота города - проходит приблизительно через центр города.
        /// </summary>
        [DataMember]
        public double Longitude { get; set; }

        /// <summary>
        /// Название города по КЛАДР.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Район по КЛАДР.
        /// </summary>
        [DataMember]
        public string District { get; set; }

        /// <summary>
        /// Регион города по КЛАДР.
        /// </summary>
        [DataMember]
        public string Region { get; set; }

        /// <summary>
        /// Суффикс населённого пункта по КЛАДР.
        /// </summary>
        [DataMember]
        public string Suffix { get; set; }

        /// <summary>
        /// Код КЛАДР.
        /// </summary>
        [DataMember]
        public string CladrCode { get; set; }

        /// <summary>
        /// Почтовый код.
        /// </summary>
        [DataMember]
        public string PostCode { get; set; }

        /// <summary>
        /// Полное составное имя города.
        /// </summary>
        [DataMember]
        public string Fullname { get; set; }
    }
}