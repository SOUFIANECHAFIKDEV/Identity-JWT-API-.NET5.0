using IdentityAPI.Contracts.V1.Requests.Queries;
using System;

namespace IdentityAPI.Servises
{
    public interface IUriService
    {
        Uri GetUserById(string postId);
        Uri GetAllUsersUri(PaginationQuery Pagination = null);
    }
}
