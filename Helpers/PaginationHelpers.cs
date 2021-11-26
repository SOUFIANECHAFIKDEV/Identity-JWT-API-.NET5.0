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
        public static PageResponse<T> CreatePaginatedResponse<T>(IUriService uriService, PaginationFilter pagination, List<T> response, bool hasNextPage, bool hasPreviousPage)
        {
            var nextPage = pagination.PageNumber >= 1
                ? uriService.GetAllUsersUri(new PaginationQuery(pagination.PageNumber + 1, pagination.PageSize)).ToString()
                : null;

            var previousPage = pagination.PageNumber - 1 >= 1
                ? uriService.GetAllUsersUri(new PaginationQuery(pagination.PageNumber + 1, pagination.PageSize)).ToString()
                : null;


            return new PageResponse<T>
            {
                Data = response,
                PageNumber = pagination.PageSize >= 1 ? pagination.PageSize : (int?)null,
                PageSize = pagination.PageSize,
                NextPage = response.Any() ? nextPage : null,
                PreviousPage = previousPage,
                HasNextPage = hasNextPage,
                HasPreviousPage = hasPreviousPage,
            };
        }
    }
}
