using IdentityAPI.Contracts.V1.Requests.Queries;
using IdentityAPI.Domain;
using AutoMapper;
using IdentityAPI.Contracts.V1.Requests;

namespace IdentityAPI.MappingProfiles
{
    public class RequestToDomaineProfile : Profile
    {
        public RequestToDomaineProfile()
        {
            CreateMap<PaginationQuery, PaginationFilter>();
            CreateMap<UpdateUserProfileRequest, ApplicationUser>();
        }
    }
}
