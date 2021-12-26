namespace IdentityAPI.Contracts.V1.Responses
{
    public class UserResponse
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CityId { get; set; }
        public CityResponse City { get; set; }
        public int LegalStatusId { get; set; }
        public LegalStatusResponse LegalStatus { get; set; }
    }

    public class CityResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class LegalStatusResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}