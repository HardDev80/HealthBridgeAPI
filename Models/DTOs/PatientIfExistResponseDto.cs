namespace HealthBridgeAPI.Models.DTOs
{
    public class PatientIfExistResponseDto
    {
        public string? PatientExternalId { get; set; }
        public string? PatientFirstName { get; set; }
        public string? PatientLastName { get; set; }
    }
}
