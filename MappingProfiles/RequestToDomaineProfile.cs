using IdentityAPI.Contracts.V1.Requests.Queries;
using IdentityAPI.Domain;
using AutoMapper;

namespace IdentityAPI.MappingProfiles
{
    public class RequestToDomaineProfile : Profile
    {
        public RequestToDomaineProfile()
        {
            CreateMap<PaginationQuery, PaginationFilter>();
        }
    }
}
