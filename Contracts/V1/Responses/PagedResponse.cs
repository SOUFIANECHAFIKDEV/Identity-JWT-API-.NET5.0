using System.Collections.Generic;

namespace IdentityAPI.Contracts.V1.Responses
{
    /// <summary>
    /// Top level data object used for response pagination
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageResponse<T>
    {
        public PageResponse()
        {

        }

        public PageResponse(IEnumerable<T> data)
        {
            Data = data;
        }

        public IEnumerable<T> Data { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string NextPage  { get; set; }
        public string PreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }
}
