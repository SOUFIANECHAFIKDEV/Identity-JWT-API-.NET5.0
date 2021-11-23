using System.Collections.Generic;
using System.Linq;
using IdentityAPI.Contracts.V1.Requests.Queries;
using IdentityAPI.Contracts.V1.Responses;
using IdentityAPI.Domain;
using IdentityAPI.Servises;

namespace IdentityAPI.Helpers
{
    public class PaginationHelpers
    {
        public static object CreatePaginatedResponse<T>(IUriService uriService, PaginationFilter pagination, List<T> response)
        {
            return new PageResponse<T>
            {
                Data = response,
                PageNumber = pagination.PageSize >= 1 ? pagination.PageSize : (int?)null,
                NextPage = null,
                PreviousPage = null
            };
        }
    }
}
