namespace HealthBridgeAPI.Models.DTOs
{
    public class CreatePractitionerRequestDto
    {
        public long PractitionerIdentifier { get; set; }
        public bool PractitionerStatus { get; set; }
        public string? PractitionerName { get; set; }
        public string? PractitionerLastName { get; set; }
        public string? PractitionerSpecialty { get; set; }
        
    }
}
