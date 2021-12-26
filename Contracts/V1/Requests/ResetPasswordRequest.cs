namespace IdentityAPI.Contracts.V1.Requests
{
    public class ResetPasswordRequest
    {
        public string UserEmail { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}