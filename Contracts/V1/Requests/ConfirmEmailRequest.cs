namespace IdentityAPI.Contracts.V1.Requests
{
    public class ConfirmEmailRequest
    {
        public string UserId { get; set; }
        public string EmailConfirmationToken { get; set; }
    }
}