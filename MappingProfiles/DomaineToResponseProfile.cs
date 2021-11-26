using IdentityAPI.Contracts.V1.Responses;
using IdentityAPI.Domain;
using AutoMapper;

namespace IdentityAPI.MappingProfiles
{
    public class DomaineToResponseProfile : Profile
    {
        public DomaineToResponseProfile()
        {
            CreateMap<ApplicationUser, UserResponse>()
                     .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
        }
    }
}