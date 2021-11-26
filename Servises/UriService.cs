using IdentityAPI.Contracts.V1;
using IdentityAPI.Contracts.V1.Requests.Queries;
using Microsoft.AspNetCore.WebUtilities;
using System;

namespace IdentityAPI.Servises
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;

        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri GetAllUsersUri(PaginationQuery pagination = null)
        {
            var uri = new Uri(_baseUri);

            if (pagination == null)
            {
                return uri;
            }

            var modifierdUri = QueryHelpers.AddQueryString(_baseUri, "pageNumber", pagination.PageNumber.ToString());
            modifierdUri = QueryHelpers.AddQueryString(modifierdUri, "pageSize", pagination.PageSize.ToString());
            return new Uri(modifierdUri);
        }

        public Uri GetUserById(string userId)
        {
            return new Uri(_baseUri + ApiRoutes.Identity.GetUserById.Replace("{userId}", userId));
        }
    }
}