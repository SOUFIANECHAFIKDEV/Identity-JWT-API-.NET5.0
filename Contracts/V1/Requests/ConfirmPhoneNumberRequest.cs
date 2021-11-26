namespace IdentityAPI.Contracts.V1.Requests
{
    public class ConfirmPhoneNumberRequest
    {
        public string UserId { get; set; }
        public string PhoneNumber { get; set; }
        public string Token { get; set; }
    }
}