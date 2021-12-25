namespace IdentityAPI.Domain
{
    public class UserRegistration
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int CityId { get; set; }
        public int LegalStatusId { get; set; }
        public string Password { get; set; }
    }
}