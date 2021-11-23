namespace IdentityAPI.Contracts.HealthChecks
{
    public class HealthChecks
    {
        public string Status { get; set; }
        public string Component { get; set; }
        public string Description { get; set; }
    }
}