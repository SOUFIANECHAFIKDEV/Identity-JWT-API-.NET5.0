namespace IdentityAPI.Contracts.V1.Requests
{
    public class SendChangeEmailConfirmationRequest
    {
        public string UserId { get; set; }
        public string NewEmail { get; set; }
    }
}