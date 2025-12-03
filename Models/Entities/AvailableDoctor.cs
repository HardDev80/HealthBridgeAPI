namespace HealthBridgeAPI.Models.Entities
{
    public class AvailableDoctor
    {
        public int PractitionerId { get; set; }
        public long PractitionerIdentifier { get; set; }
        public string? PractitionerName { get; set; }
        public string? PractitionerLastName { get; set; }
        public string? PractitionerPhotoRef { get; set; }
        public string? PractitionerSpecialty { get; set; }
        public string? PractitionerProvider { get; set; }
        public string? PractitionerLocation { get; set; }
        public DateTime LastRefreshed { get; set; }
    }
}
