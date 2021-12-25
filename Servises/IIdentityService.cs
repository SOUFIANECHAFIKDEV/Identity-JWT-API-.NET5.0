using IdentityAPI.Domain;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityAPI.Servises
{
    public interface IIdentityService
    {
        public Task<AuthenticationResult> RegisterAsync(UserRegistration newUser);
        public Task<Result> ConfirmEmailAsync(string UserId, string EmailConfirmationToken);

        public Task<AuthenticationResult> LoginAsync(string email, string password);
        public Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);

       
        public Task<Result> ChangeEmailAsync(string UserId, string NewEmail, string token);
        public Task<Result> SendChangeEmailConfirmationAsync(string UserId, string NewEmail);

        public Task<Result> ResetPasswordAsync(string UserId, string token, string newPassword);
        public Task<Result> SendResetPasswordConfirmationEmailAsync(string UserId);

        public IQueryable<ApplicationUser> GetAllUsers();
        public Task<ApplicationUser> GetUserByIdAsync(string UserId);
        public Task<bool> UpdateUserProfileAsync(ApplicationUser userToUpdate);
        public Task<bool> DeleteUserAsync(ApplicationUser user);

        public IQueryable<IdentityRole> GetRolesList();
        public Task<UserRolesResult> GetUserRolesByIdAsync(string userId);
        public Task<AddUserToRolesResult> AddUserToRolesAsync(string email, IEnumerable<string> roles);
        
        
        public List<ApplicationFeatures> GetApplicationFeatures();
        public Task<GetAuthorizedFeaturesResult> GetUserAuthorizedFeaturesAsync(string UserId);
        public Task<Result> UpdateAuthorizedFeaturesAsync(string UserId, List<string> appFeatures);

        public Task<Result> ConfirmPhoneNumberAsync(string UserId, string phoneNumber, string token);
        public Task<Result> SendPhoneVerificationAsync(string UserId, string phoneNumber);
    }
}