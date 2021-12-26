using IdentityAPI.Constants;
using IdentityAPI.Data;
using IdentityAPI.Domain;
using IdentityAPI.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using static Twilio.Rest.Api.V2010.Account.Call.FeedbackSummaryResource;

namespace IdentityAPI.Servises
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly DataContext _dataContext;
        private readonly MailSettings _mailSettings;
        private readonly ITwilioRestClient _client;
        public IdentityService(
            UserManager<ApplicationUser> userManager,
            JwtSettings jwtSettings,
            TokenValidationParameters tokenValidationParameters,
            DataContext dataContext,
            IOptions<MailSettings> mailSettings, ITwilioRestClient client)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _tokenValidationParameters = tokenValidationParameters;
            _dataContext = dataContext;
            _mailSettings = mailSettings.Value;
            _client = client;
        }
        public async Task<AuthenticationResult> RegisterAsync(UserRegistration newUser)
        {
            #region check if the user email alerady registed
            var existingUser = await _userManager.FindByEmailAsync(newUser.Email);

            if (existingUser != null)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Errors = new[] { "User with this email adress already exists" }
                };
            }
            #endregion

            #region save the user in the database
            var newApplicationUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = newUser.Email,
                UserName = newUser.Email,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                PhoneNumber = newUser.PhoneNumber,
                CityId = newUser.CityId,
                LegalStatusId = newUser.LegalStatusId,
            };

            var createdUser = await _userManager.CreateAsync(newApplicationUser, newUser.Password);

            if (!createdUser.Succeeded)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Errors = createdUser.Errors.Select(x => x.Description)
                };
            }
            #endregion

            #region send email to the user for confirmation
            /* var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
             var emailBuilder = new EmailBuilder();
             emailBuilder.MailTo = newUser.Email;
             emailBuilder.Subject = "Email Confirmation";
             emailBuilder.Body = emailConfirmationToken; //ToDO
             await SendEmailAsync(emailBuilder);*/
            #endregion

            #region assign the default user roles
            await _userManager.AddToRoleAsync(newApplicationUser, Roles.Admin);
            #endregion

            #region assign the default Authorized application features claims to the user
            //await _userManager.AddClaimAsync(newApplicationUser, new Claim(type: ApplicationFeaturesClaims.ClaimName, value: ApplicationFeaturesClaims.Module1.Feature1));
            //await _userManager.AddClaimAsync(newApplicationUser, new Claim(type: ApplicationFeaturesClaims.ClaimName, value: ApplicationFeaturesClaims.Module2.Feature1));
            #endregion

            return await GenerateAuthenticationResultForUserAsync(newApplicationUser);
        }

        public async Task<Result> ConfirmEmailAsync(string UserId, string EmailConfirmationToken)
        {
            var user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
            {
                return new Result
                {
                    Success = false,
                    Errors = new[] { "User does not exist" }
                };
            }

            var result = await _userManager.ConfirmEmailAsync(user, EmailConfirmationToken);

            if (!result.Succeeded)
            {
                return new Result
                {
                    Success = false,
                    Errors = result.Errors.Select(x => x.Description)
                };
            }

            return new Result { Success = true };
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Errors = new[] { "User does not exist" }
                };
            }

            var userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);

            if (!userHasValidPassword)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Errors = new[] { "User/password combination is wrong" }
                };
            }

            return await GenerateAuthenticationResultForUserAsync(user);
        }

        public async Task<Result> SendChangeEmailConfirmationAsync(string UserId, string NewEmail)
        {
            var user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
            {
                return new Result
                {
                    Success = false,
                    Errors = new[] { "User does not exist" }
                };
            }

            #region send email to the user for confirmation
            var changeEmailToken = await _userManager.GenerateChangeEmailTokenAsync(user, NewEmail);
            var emailBuilder = new EmailBuilder();
            emailBuilder.MailTo = user.Email;
            emailBuilder.Subject = "Change Email Confirmation";
            emailBuilder.Body = changeEmailToken; //ToDO
            await SendEmailAsync(emailBuilder);

            #endregion
            return new Result { Success = true };
        }

        public async Task<Result> ChangeEmailAsync(string UserId, string NewEmail, string token)
        {
            var user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
            {
                return new Result
                {
                    Success = false,
                    Errors = new[] { "User does not exist" }
                };
            }
            var result = await _userManager.ChangeEmailAsync(user, NewEmail, token);

            if (!result.Succeeded)
            {
                return new Result
                {
                    Success = false,
                    Errors = result.Errors.Select(x => x.Description)
                };
            }

            return new Result { Success = true };
        }

        public async Task<Result> SendResetPasswordConfirmationEmailAsync(string UserEmail)
        {
            var user = await _userManager.FindByEmailAsync(UserEmail);

            if (user == null)
            {
                return new Result
                {
                    Success = false,
                    Errors = new[] { "User does not exist" }
                };
            }

            #region send email to the user for confirmation
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var emailBuilder = new EmailBuilder();
            emailBuilder.MailTo = user.Email;
            emailBuilder.Subject = "Change Email Confirmation";
            emailBuilder.Body = token; //ToDO
            await SendEmailAsync(emailBuilder);

            #endregion
            return new Result { Success = true };
        }

        public async Task<Result> ResetPasswordAsync(string UserEmail, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(UserEmail);

            if (user == null)
            {
                return new Result
                {
                    Success = false,
                    Errors = new[] { "User does not exist" }
                };
            }

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, newPassword);

            if (isPasswordCorrect)
            {
                return new Result
                {
                    Success = false,
                    Errors = new[] { "the user not changed the passord" }
                };
            }
            user.UserName = user.Email;
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (!result.Succeeded)
            {
                return new Result
                {
                    Success = false,
                    Errors = result.Errors.Select(x => x.Description).ToList()
                };
            }

            return new Result { Success = true };
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken)
        {
            var validatedToken = GetPricipalFromToken(token);

            if (validatedToken == null)
            {
                return new AuthenticationResult { Errors = new[] { "Invalid Token" } };
            }

            var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            if (expiryDateTimeUtc > DateTime.UtcNow)
            {
                return new AuthenticationResult { Errors = new[] { "this token hasn't expired yet" } };
            }

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = await _dataContext.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);

            if (storedRefreshToken == null)
            {
                return new AuthenticationResult { Errors = new[] { "this refresh token does not exist" } };
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                return new AuthenticationResult { Errors = new[] { "this refresh token has exprired" } };
            }

            if (storedRefreshToken.Invalidated)
            {
                return new AuthenticationResult { Errors = new[] { "this refresh token has been invalidated" } };
            }

            if (storedRefreshToken.Used)
            {
                return new AuthenticationResult { Errors = new[] { "this refresh token has been used" } };
            }

            if (storedRefreshToken.JwtId != jti)
            {
                return new AuthenticationResult { Errors = new[] { "this refresh token does not match this JWT" } };
            }

            storedRefreshToken.Used = true;
            _dataContext.RefreshTokens.Update(storedRefreshToken);
            await _dataContext.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "id").Value);
            return await GenerateAuthenticationResultForUserAsync(user);
        }

        public async Task<AddUserToRolesResult> AddUserToRolesAsync(string email, IEnumerable<string> roles)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new AddUserToRolesResult
                {
                    Success = false,
                    Errors = new[] { "User does not exist" }
                };
            }

            IdentityResult result = await _userManager.AddToRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                return new AddUserToRolesResult
                {
                    Success = false,
                    Errors = result.Errors.Select(x => x.Description).ToArray<string>()
                };
            }


            return new AddUserToRolesResult
            {
                Success = true
            };
        }

        private async Task<AuthenticationResult> GenerateAuthenticationResultForUserAsync(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);//Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            /*var claims = new List<Claim>
                {
                    new Claim(type: JwtRegisteredClaimNames.Sub, value: user.Email),
                    new Claim(type: JwtRegisteredClaimNames.Jti, value: Guid.NewGuid().ToString()),
                    new Claim(type: JwtRegisteredClaimNames.Email, value: user.Email),
                    new Claim(type: "id", value: user.Id),
                };

            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            var userRoles = await _userManager.GetRolesAsync(user);
            claims.Add(new Claim(type: "roles", value: userRoles.ToString()));*/

            var claims = await GetValidClaims(user);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(_jwtSettings.DurationInDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescription);

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };

            await _dataContext.RefreshTokens.AddAsync(refreshToken);
            await _dataContext.SaveChangesAsync();

            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token
            };
        }

        private ClaimsPrincipal GetPricipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validateToken);

                if (!IsJwtWithValidSecurityAlgorithm(validateToken))
                {
                    return null;
                }
                else
                {
                    return principal;
                }
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<ApplicationUser> GetAllUsers()
        {
            return _dataContext.Users.AsQueryable<ApplicationUser>();
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string UserId)
        {
            return await _dataContext.Users.Include(x => x.City).Include(x => x.LegalStatus).SingleOrDefaultAsync(x => x.Id == UserId);
        }

        public async Task<bool> UpdateUserProfileAsync(ApplicationUser userToUpdate)
        {
            _dataContext.Users.Update(userToUpdate);
            var updated = await _dataContext.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<bool> DeleteUserAsync(ApplicationUser user)
        {
            _dataContext.Users.Remove(user);
            var deleted = await _dataContext.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<Result> UpdateAuthorizedFeaturesAsync(string UserId, List<string> appFeatures)
        {
            #region block 1
            var user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
            {
                return new Result
                {
                    Success = false,
                    Errors = new[] { "User does not exist" }
                };
            }
            #endregion

            #region block 2
            appFeatures.Sort();
            GetAllAppFeatures().Sort();
            if (!GetAllAppFeatures().SequenceEqual(appFeatures))
            {
                return new Result
                {
                    Success = false,
                    Errors = new[] { "Some features that you want to add does not exist" }
                };
            }
            #endregion

            #region block 3
            var userClaims = await _userManager.GetClaimsAsync(user);

            var userAppFeaturesClaims = userClaims.Where(x => x.Type == ApplicationFeaturesClaims.ClaimName).Select(x => x.Value).ToList();

            var removeClaimsResult = await _userManager.RemoveClaimsAsync(user, userAppFeaturesClaims.Select(feature => new Claim(type: ApplicationFeaturesClaims.ClaimName, value: feature)));

            if (!removeClaimsResult.Succeeded)
            {
                return new Result
                {
                    Success = false,
                    Errors = removeClaimsResult.Errors.Select(x => x.Description)
                };
            }
            #endregion

            #region block 4
            var claimsToAdd = appFeatures.Select(feature => new Claim(type: ApplicationFeaturesClaims.ClaimName, value: feature));
            var result = await _userManager.AddClaimsAsync(user, claimsToAdd);

            if (!result.Succeeded)
            {
                return new Result
                {
                    Success = false,
                    Errors = result.Errors.Select(x => x.Description)
                };
            }

            return new Result { Success = true };
            #endregion
        }

        public List<ApplicationFeatures> GetApplicationFeatures()
        {
            var appModules = new List<ApplicationFeatures>();

            foreach (var appModule in typeof(ApplicationFeaturesClaims).GetNestedTypes())
            {
                var ModuleFeatures = appModule.GetConstants().Select(feature => feature.GetRawConstantValue().ToString()).ToList();
                appModules.Add(new ApplicationFeatures
                {
                    AppModule = appModule.Name,
                    Features = ModuleFeatures
                });
            }

            return appModules;
        }

        public IQueryable<IdentityRole> GetRolesList()
        {
            return _dataContext.Roles.Where(x => x.Name != Roles.SuperAdmin).AsQueryable<IdentityRole>();
        }

        public async Task<GetAuthorizedFeaturesResult> GetUserAuthorizedFeaturesAsync(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
            {
                return new GetAuthorizedFeaturesResult
                {
                    Success = false,
                    Errors = new[] { "User does not exist" }
                };
            }

            var userClaims = await _userManager.GetClaimsAsync(user);

            var userAppFeaturesClaims = userClaims.Where(x => x.Type == ApplicationFeaturesClaims.ClaimName).Select(x => x.Value).ToList();
            List<string> allAppfeatures = GetAllAppFeatures();

            var FeaturesAuthorizations = new List<FeatureAuthorization>();

            FeaturesAuthorizations.AddRange(allAppfeatures.Select(feature => new FeatureAuthorization
            {
                Feature = feature,
                Authorized = userAppFeaturesClaims.Contains(feature)
            }).ToList());

            return new GetAuthorizedFeaturesResult { Success = true, FeaturesAuthorizations = FeaturesAuthorizations };
        }

        private static List<string> GetAllAppFeatures()
        {
            var allAppfeatures = new List<string>();

            foreach (var appModule in typeof(ApplicationFeaturesClaims).GetNestedTypes())
            {
                var ModuleFeatures = appModule.GetConstants().Select(feature => feature.GetRawConstantValue().ToString()).ToList();
                allAppfeatures.AddRange(ModuleFeatures);
            }

            return allAppfeatures;
        }

        public async Task<UserRolesResult> GetUserRolesByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new UserRolesResult
                {
                    Success = false,
                    Errors = new[] { "User does not exist" }
                };
            }

            List<IdentityUserRole<string>> userRoles = await _dataContext.UserRoles.Where(x => x.UserId == userId).ToListAsync();

            List<IdentityRole> roles = await _dataContext.Roles.ToListAsync();

            var allRoles =  roles.Where(x => userRoles.Select(ur => ur.RoleId).Contains(x.Id)).ToList();

            return new UserRolesResult
            {
                Success = true,
                Roles = allRoles
            };
        }

        public async Task<Result> ConfirmPhoneNumberAsync(string UserId, string phoneNumber, string token)
        {
            var user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
            {
                return new Result
                {
                    Success = false,
                    Errors = new[] { "User does not exist" }
                };
            }

            if (await _userManager.IsPhoneNumberConfirmedAsync(user))
            {
                return new Result
                {
                    Success = false,
                    Errors = new[] { "Phone Number alerady confirmed" }
                };
            }

            if (!await _userManager.VerifyChangePhoneNumberTokenAsync(user, token, phoneNumber))
            {
                return new Result
                {
                    Success = false,
                    Errors = new[] { "verification Token incorrect" }
                };
            }

            var result = await _userManager.ChangePhoneNumberAsync(user, phoneNumber, token);

            if (!result.Succeeded)
            {
                return new Result
                {
                    Success = false,
                    Errors = result.Errors.Select(x => x.Description)
                };
            }

            return new Result { Success = true };
        }

        public async Task<Result> SendPhoneVerificationAsync(string UserId, string phoneNumber)
        {
            var user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
            {
                return new Result
                {
                    Success = false,
                    Errors = new[] { "User does not exist" }
                };
            }

            var smsMessage = new SmsMessage();

            smsMessage.To = await _userManager.GetPhoneNumberAsync(user);
            smsMessage.From = "User Identity";
            var generatedToken = await _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
            smsMessage.Message = $"Your confirmation token ${generatedToken}";
            var sendSmsResult = await SendSms(smsMessage);

            if (sendSmsResult.Status == StatusEnum.Failed)
            {
                return new Result
                {
                    Success = false,
                    Errors = new string[] { sendSmsResult.ErrorMessage }
                };
            }

            return new Result { Success = true };
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                    jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
        }

        private async Task<List<Claim>> GetValidClaims(ApplicationUser user)
        {
            IdentityOptions _options = new IdentityOptions();
            var claims = new List<Claim>
                        {
                            new Claim(type: JwtRegisteredClaimNames.Sub, value: user.Email),
                            new Claim(type: JwtRegisteredClaimNames.Jti, value: Guid.NewGuid().ToString()),
                            new Claim(type: JwtRegisteredClaimNames.Email, value: user.Email),
                            new Claim(type: "id", value: user.Id),
                        };

            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _userManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    var roleClaims = await _userManager.GetClaimsAsync(role);
                    foreach (Claim roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }
            return claims;
        }

        private async Task SendEmailAsync(EmailBuilder emailBuilder)
        {
            #region create email
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Email),
                Subject = emailBuilder.Subject,
            };
            email.To.Add(MailboxAddress.Parse(emailBuilder.MailTo));
            var builder = new BodyBuilder();
            if (emailBuilder.Attachments != null) SetAttachments(emailBuilder.Attachments, ref builder);
            builder.HtmlBody = emailBuilder.Body;
            email.Body = builder.ToMessageBody();
            //email.Cc.AddRange(emailBuilder.Cc.Select(c => MailboxAddress.Parse(c)).ToList());
            //email.Bcc.AddRange(emailBuilder.Bcc.Select(c => MailboxAddress.Parse(c)).ToList());
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Email));
            #endregion

            #region send the email
            using var smpt = new SmtpClient();

            //SecureSocketOptions options = new SecureSocketOptions
            //{

            //}:

            smpt.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smpt.Authenticate(_mailSettings.Email, _mailSettings.Password);
            await smpt.SendAsync(email);
            #endregion

            smpt.Disconnect(true);
        }

        private void SetAttachments(IList<IFormFile> attachments, ref BodyBuilder builder)
        {
            byte[] fileBytes;
            foreach (var file in attachments)
            {
                if (file.Length > 0)
                {
                    using var ms = new MemoryStream();
                    file.CopyTo(ms);
                    fileBytes = ms.ToArray();
                    builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                }
            }
        }

        private async Task<MessageResource> SendSms(SmsMessage model)
        {
            var message = await MessageResource.CreateAsync(
                to: new PhoneNumber(model.To),
                from: new PhoneNumber(model.From),
                body: model.Message,
                client: _client);

            return message;
        }
    }
}
