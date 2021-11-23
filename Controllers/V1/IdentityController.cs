using Microsoft.AspNetCore.Mvc;
using IdentityAPI.Servises;
using IdentityAPI.Contracts.V1;
using System.Threading.Tasks;
using IdentityAPI.Contracts.V1.Requests;
using IdentityAPI.Contracts.V1.Responses;

namespace IdentityAPI.Controllers.V1
{
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }
    }
}
