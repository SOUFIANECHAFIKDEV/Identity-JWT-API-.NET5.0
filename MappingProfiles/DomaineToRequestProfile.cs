using IdentityAPI.Domain;
using AutoMapper;
using IdentityAPI.Contracts.V1.Requests;

namespace IdentityAPI.MappingProfiles
{
    public class DomaineToRequestProfile : Profile
    {
        public DomaineToRequestProfile()
        {
            CreateMap<UserRegistrationRequest, UserRegistration>();
        }
    }
}