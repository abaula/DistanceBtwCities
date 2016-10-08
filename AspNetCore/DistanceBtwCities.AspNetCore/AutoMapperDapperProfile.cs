using AutoMapper;
using DistanceBtwCities.Common.Dtos;
using DistanceBtwCities.Dal.Common;
using DistanceBtwCities.Dal.DataObjects;

namespace DistanceBtwCities.AspNetCore
{
    public class AutoMapperDapperProfile : Profile
    {
        public AutoMapperDapperProfile()
        {
            CreateMap<CityInfoDo, CityInfo>()
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => (decimal)src.latitude / (decimal)GeoConstants.GeoFactor))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => (decimal)src.longitude / (decimal)GeoConstants.GeoFactor))
                .ForMember(dest => dest.CladrCode, opt => opt.MapFrom(src => src.cladr_code));

            CreateMap<RouteInfoDo, RouteInfo>()
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
        }
    }
}
