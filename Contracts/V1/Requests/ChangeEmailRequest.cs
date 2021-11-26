namespace IdentityAPI.Contracts.V1.Requests
{
    public class ChangeEmailRequest
    {
        public string UserId { get; set; }
        public string NewEmail { get; set; }
        public string Token { get; set; }
    }
}