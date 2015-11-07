using System.Runtime.Remoting;
using System.Runtime.Serialization;

namespace DistanceBtwCities.DataContract
{
    [DataContract]
    public class CityInfo
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public double Latitude { get; set; }
        [DataMember]
        public double Longitude { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string District { get; set; }
        [DataMember]
        public string Region { get; set; }
        [DataMember]
        public string Suffix { get; set; }
        [DataMember]
        public string CladrCode { get; set; }
        [DataMember]
        public string PostCode { get; set; }
        [DataMember]
        public string Fullname { get; set; }
    }
}
