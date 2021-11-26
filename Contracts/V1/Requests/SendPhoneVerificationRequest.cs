namespace IdentityAPI.Contracts.V1.Requests
{
    public class SendPhoneVerificationRequest
    {
        public string UserId { get; set; }
        public string PhoneNumber { get; set; }
    }
}