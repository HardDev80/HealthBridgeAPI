using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace HealthBridgeAPI.Models.Entities
{
    public class Practitioner
    {
        [Key]
        public int PractitionerId { get; set; }
        public long PractitionerIdentifier { get; set; }
        public bool PractitionerStatus { get; set; }
        [Required]
        public string? PractitionerName { get; set; }
        public string? PractitionerLastName { get; set; }
        public string? PractitionerPhotoRef { get; set; }
        public string? PractitionerSpecialty { get; set; }
        public string? PractitionerProvider {  get; set; }
        public bool? PractitionerOnLine { get; set; }
        public string? PractitionerLocation { get; set; }

    }
}
