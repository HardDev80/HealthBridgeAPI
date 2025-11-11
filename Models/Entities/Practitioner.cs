using System.Numerics;

namespace HealthBridgeAPI.Models.Entities
{
    public class Practitioner
    {
        public BigInteger PractitionerId { get; set; }
        public BigInteger PractitionerIdentifier { get; set; }
        public Boolean PractitionerStatus { get; set; }
        public string PractitionerName { get; set; }
        public string PractitionerPhotoRef { get; set; }
        public string PractitionerSpecialty { get; set; }
        public string PractitionerProvider {  get; set; }

    }
}
