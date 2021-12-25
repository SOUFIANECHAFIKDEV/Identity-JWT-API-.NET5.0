using Api.Extensions;
using AutoMapper;
using IdentityAPI.Constants;
using IdentityAPI.Contracts.V1;
using IdentityAPI.Contracts.V1.Requests;
using IdentityAPI.Contracts.V1.Requests.Queries;
using IdentityAPI.Contracts.V1.Responses;
using IdentityAPI.Domain;
using IdentityAPI.Helpers;
using IdentityAPI.Servises;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityAPI.Controllers.V1
{
    [Produces("application/json")]
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public IdentityController(IIdentityService identityService, IMapper mapper, IUriService uriService)
        {
            _identityService = identityService;
            _mapper = mapper;
            _uriService = uriService;
        }

        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            //UserRegistration, UserRegistrationRequest
            var userRegistration = _mapper.Map<UserRegistration>(request);

            var authResponse = await _identityService.RegisterAsync(userRegistration);

            if (!authResponse.Success)
            {
                return BadRequest(new IdentityFaildResponse
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok(new UserRegistrationSuccessResponse {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            });
        }

        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var authResponse = await _identityService.LoginAsync(request.Email, request.Password);

            if (!authResponse.Success)
            {
                return BadRequest(new IdentityFaildResponse
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
        }

        [HttpPost(ApiRoutes.Identity.Refresh)]
        public async Task<IActionResult> Login([FromBody] RefreshTokenRequest request)
        {
            var authResponse = await _identityService.RefreshTokenAsync(request.Token, request.RefreshToken);

            if (!authResponse.Success)
            {
                return BadRequest(new IdentityFaildResponse
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles.SuperAdmin)]
        [HttpPost(ApiRoutes.Identity.AddUserToRoles)]
        public async Task<IActionResult> AddUserToRoles([FromBody] AddUserToRolesRequest request)
        {
            var authResponse = await _identityService.AddUserToRolesAsync(request.Email, request.Roles);

            if (!authResponse.Success)
            {
                return BadRequest(new AddUserToRolesFaildResponse
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok();
        }

        [HttpPost(ApiRoutes.Identity.ConfirmEmail)]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
        {
            var confirmEmailResponse = await _identityService.ConfirmEmailAsync(request.UserId, request.EmailConfirmationToken);

            if (!confirmEmailResponse.Success)
            {
                return BadRequest(new IdentityFaildResponse
                {
                    Errors = confirmEmailResponse.Errors
                });
            }

            return Ok();
        }

        [HttpPost(ApiRoutes.Identity.ChangeEmail)]
        public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailRequest request)
        {
            var changeEmailResponse = await _identityService.ChangeEmailAsync(request.UserId, request.NewEmail, request.Token);

            if (!changeEmailResponse.Success)
            {
                return BadRequest(new IdentityFaildResponse
                {
                    Errors = changeEmailResponse.Errors
                });
            }

            return Ok();
        }

        [HttpPost(ApiRoutes.Identity.SendChangeEmailConfirmation)]
        public async Task<IActionResult> SendChangeEmailConfirmation([FromBody] SendChangeEmailConfirmationRequest request)
        {
            var sendChangeEmailConfirmationResponse = await _identityService.SendChangeEmailConfirmationAsync(request.UserId, request.NewEmail);

            if (!sendChangeEmailConfirmationResponse.Success)
            {
                return BadRequest(new IdentityFaildResponse
                {
                    Errors = sendChangeEmailConfirmationResponse.Errors
                });
            }

            return Ok();
        }

        [HttpPost(ApiRoutes.Identity.ResetPassword)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var resetPasswordResponse = await _identityService.ResetPasswordAsync(request.UserId, request.Token, request.NewPassword);

            if (!resetPasswordResponse.Success)
            {
                return BadRequest(new IdentityFaildResponse
                {
                    Errors = resetPasswordResponse.Errors
                });
            }

            return Ok();
        }

        [HttpGet(ApiRoutes.Identity.SendResetPasswordConfirmationEmail)]
        public async Task<IActionResult> SendResetPasswordConfirmationEmail([FromRoute] string userId)
        {
            var sendResetPasswordConfirmationEmailResponse = await _identityService.SendResetPasswordConfirmationEmailAsync(userId);

            if (!sendResetPasswordConfirmationEmailResponse.Success)
            {
                return BadRequest(new IdentityFaildResponse
                {
                    Errors = sendResetPasswordConfirmationEmailResponse.Errors
                });
            }

            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles.SuperAdmin)]
        [HttpPost(ApiRoutes.Identity.GetAllUsers)]
        public IActionResult GetAllUsers([FromQuery] PaginationQuery paginationQuery = null)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);

            var allUsersQuery = _identityService.GetAllUsers();

            var usersResponse = new List<UserResponse>();

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                usersResponse = _mapper.Map<List<UserResponse>>(allUsersQuery.ToList());
                return Ok(new PageResponse<UserResponse>(usersResponse));
            }

            var skip = (paginationQuery.PageNumber - 1) * paginationQuery.PageSize;
            var users = allUsersQuery.Skip(skip).Take(paginationQuery.PageSize).ToList();
            usersResponse = _mapper.Map<List<UserResponse>>(users);

            #region get hasNextPage and hasPreviousPage
            var nextPageUsersSkip = paginationQuery.PageNumber * paginationQuery.PageSize;
            var nextPageUsersCount = allUsersQuery.Skip(nextPageUsersSkip).Take(paginationQuery.PageSize).Count();
            var hasNextPage = nextPageUsersCount > 0;
            var hasPreviousPage = pagination.PageNumber > 1;
            #endregion

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, usersResponse, hasNextPage, hasPreviousPage);

            return Ok(paginationResponse);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles.SuperAdmin)]
        [HttpGet(ApiRoutes.Identity.GetUserById)]
        public async Task<IActionResult> GetUserById([FromRoute] string userId)
        {
            var user = await _identityService.GetUserByIdAsync(userId);

            if (user == null)
                return NotFound();

            return Ok(new Response<UserResponse>(_mapper.Map<UserResponse>(user)));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles.SuperAdmin)]
        [HttpPost(ApiRoutes.Identity.UpdateUserProfile)]
        public async Task<IActionResult> UpdateUserProfile([FromRoute] string userId, [FromBody] UpdateUserProfileRequest request)
        {
            var user = await _identityService.GetUserByIdAsync(userId);

            if (user == null)
                return BadRequest(new { error = "user does not exists" });

            var userToUpdate = _mapper.Map<ApplicationUser>(request);

            userToUpdate.Id = userId;

            var updateUserProfileResponse = await _identityService.UpdateUserProfileAsync(userToUpdate);

            if (updateUserProfileResponse)
                return Ok(new Response<UserResponse>(_mapper.Map<UserResponse>(userToUpdate)));

            return NotFound();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles.SuperAdmin)]
        [HttpGet(ApiRoutes.Identity.DeleteUserProfile)]
        public async Task<IActionResult> DeleteUser([FromRoute] string userId)
        {
            if (HttpContext.GetUserId() == userId)
                return BadRequest(new { error = "You can not delete your own Account" });

            var user = await _identityService.GetUserByIdAsync(userId);

            if (user == null)
                return BadRequest(new { error = "user does not exists" });

            var deleteUserResponse = await _identityService.DeleteUserAsync(user);

            if (deleteUserResponse)
                return NoContent();

            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles.SuperAdmin)]
        [HttpGet(ApiRoutes.Identity.GetRolesList)]
        public IActionResult GetRolesList()
        {
            var getRolesListResponse = _identityService.GetRolesList();

            return Ok(getRolesListResponse.ToList());
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles.SuperAdmin)]
        [HttpGet(ApiRoutes.Identity.GetUserRolesById)]
        public async Task<IActionResult> GetUserRolesById([FromRoute] string userId)
        {
            var sendResetPasswordConfirmationEmailResponse = await _identityService.GetUserRolesByIdAsync(userId);

            if (!sendResetPasswordConfirmationEmailResponse.Success)
            {
                return BadRequest(new IdentityFaildResponse
                {
                    Errors = sendResetPasswordConfirmationEmailResponse.Errors
                });
            }

            return Ok(sendResetPasswordConfirmationEmailResponse.Roles);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles.SuperAdmin)]
        [HttpGet(ApiRoutes.Identity.GetApplicationFeatures)]
        public IActionResult GetApplicationFeatures()
        {
            var applicationFeaturesResponse = _identityService.GetApplicationFeatures();

            return Ok(applicationFeaturesResponse);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles.SuperAdmin)]
        [HttpGet(ApiRoutes.Identity.GetUserAuthorizedFeatures)]
        public async Task<IActionResult> GetUserAuthorizedFeatures()
        {
            var userAuthorizedFeaturesResponse = await _identityService.GetUserAuthorizedFeaturesAsync(HttpContext.GetUserId());

            if (!userAuthorizedFeaturesResponse.Success)
            {
                return BadRequest(new IdentityFaildResponse
                {
                    Errors = userAuthorizedFeaturesResponse.Errors
                });
            }

            return Ok(userAuthorizedFeaturesResponse.FeaturesAuthorizations);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles.SuperAdmin)]
        [HttpPost(ApiRoutes.Identity.UpdateAuthorizedFeatures)]
        public async Task<IActionResult> UpdateAuthorizedFeatures([FromBody] UpdateAuthorizedFeaturesRequest request)
        {
            var updateAuthorizedFeaturesAsyncResponse = await _identityService.UpdateAuthorizedFeaturesAsync(request.UserId, request.AppFeatures);

            if (!updateAuthorizedFeaturesAsyncResponse.Success)
            {
                return BadRequest(new IdentityFaildResponse
                {
                    Errors = updateAuthorizedFeaturesAsyncResponse.Errors
                });
            }

            return Ok();
        }

        [HttpPost(ApiRoutes.Identity.ConfirmPhoneNumber)]
        public async Task<IActionResult> ConfirmPhoneNumber([FromBody] ConfirmPhoneNumberRequest request)
        {
            var confirmPhoneNumberResponse = await _identityService.ConfirmPhoneNumberAsync(request.UserId, request.PhoneNumber, request.Token);

            if (!confirmPhoneNumberResponse.Success)
            {
                return BadRequest(new IdentityFaildResponse
                {
                    Errors = confirmPhoneNumberResponse.Errors
                });
            }

            return Ok();
        }

        [HttpPost(ApiRoutes.Identity.SendPhoneVerification)]
        public async Task<IActionResult> SendPhoneVerification([FromBody] SendPhoneVerificationRequest request)
        {
            var sendPhoneVerificationResponse = await _identityService.SendPhoneVerificationAsync(request.UserId, request.PhoneNumber);

            if (!sendPhoneVerificationResponse.Success)
            {
                return BadRequest(new IdentityFaildResponse
                {
                    Errors = sendPhoneVerificationResponse.Errors
                });
            }

            return Ok();
        }

        [HttpGet(ApiRoutes.Identity.GetUserOwnsProfile)]
        public async Task<IActionResult> GetUserOwnsProfile()
        {
            var user = await _identityService.GetUserByIdAsync(HttpContext.GetUserId());

            if (user == null)
                return NotFound();

            return Ok(new Response<UserResponse>(_mapper.Map<UserResponse>(user)));
        }

        [HttpPost(ApiRoutes.Identity.UpdateUserOwnsProfile)]
        public async Task<IActionResult> UpdateUserOwnsProfile([FromBody] UpdateUserProfileRequest request)
        {
            var user = await _identityService.GetUserByIdAsync(HttpContext.GetUserId());

            if (user == null)
                return BadRequest(new { error = "user does not exists" });

            var userToUpdate = _mapper.Map<ApplicationUser>(request);

            userToUpdate.Id = HttpContext.GetUserId();

            var updateUserProfileResponse = await _identityService.UpdateUserProfileAsync(userToUpdate);

            if (updateUserProfileResponse)
                return Ok(new Response<UserResponse>(_mapper.Map<UserResponse>(userToUpdate)));

            return NotFound();
        }
    }
}
