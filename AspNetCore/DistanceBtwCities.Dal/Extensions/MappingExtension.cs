using AutoMapper;
using DistanceBtwCities.Common.Dtos;
using DistanceBtwCities.Dal.DataObjects;
using DistanceBtwCities.Dal.Common;

namespace DistanceBtwCities.Dal.Extensions
{
    public static class MappingExtension
    {
        private static readonly IMapper Mapper;

        static MappingExtension()
        {
            var config = new MapperConfiguration(cfg => 
            {
                cfg.CreateMap<CityInfoDo, CityInfo>()
                    .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => (decimal)src.latitude / (decimal)GeoConstants.GeoFactor))
                    .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => (decimal)src.longitude / (decimal)GeoConstants.GeoFactor))
                    .ForMember(dest => dest.CladrCode, opt => opt.MapFrom(src => src.cladr_code));

                cfg.CreateMap<RouteInfoDo, RouteInfo>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.geoRouteId))
                    .ForMember(dest => dest.City1, opt => opt.MapFrom(src => new CityInfo
                    {
                        Id = src.cityId1,
                        Latitude = (decimal)src.cityLatitude1 / (decimal)GeoConstants.GeoFactor,
                        Longitude = (decimal)src.cityLongitude1 / (decimal)GeoConstants.GeoFactor,
                        Name = src.cityName1,
                        FullName = src.cityfullName1
                    }))
                    .ForMember(dest => dest.City2, opt => opt.MapFrom(src => new CityInfo
                    {
                        Id = src.cityId2,
                        Latitude = (decimal)src.cityLatitude2 / (decimal)GeoConstants.GeoFactor,
                        Longitude = (decimal)src.cityLongitude2 / (decimal)GeoConstants.GeoFactor,
                        Name = src.cityName2,
                        FullName = src.cityfullName2
                    }));
            });

            config.AssertConfigurationIsValid();
            Mapper = config.CreateMapper();
        }

        public static CityInfo ToCityInfo(this CityInfoDo data)
        {
            return Mapper.Map<CityInfoDo, CityInfo>(data);
        }

        public static RouteInfo ToRouteInfo(this RouteInfoDo data)
        {
            return Mapper.Map<RouteInfoDo, RouteInfo>(data);
        }
    }
}
