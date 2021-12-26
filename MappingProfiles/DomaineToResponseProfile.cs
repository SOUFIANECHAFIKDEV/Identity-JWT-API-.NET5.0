using IdentityAPI.Contracts.V1.Responses;
using IdentityAPI.Domain;
using AutoMapper;

namespace IdentityAPI.MappingProfiles
{
    public class DomaineToResponseProfile : Profile
    {
        public DomaineToResponseProfile()
        {
            CreateMap<City, CityResponse>();
            CreateMap<LegalStatus, LegalStatusResponse>();
            CreateMap<ApplicationUser, UserResponse>()
                     .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                     /*.ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                     .ForMember(dest => dest.LegalStatus, opt => opt.MapFrom(src => src.LegalStatus))*/;
        }
    }
}