using System.ComponentModel.DataAnnotations;

namespace HealthBridgeAPI.Models.Entities
{
    public class Patient
    {
        public int PatientId { get; set; }
        [Required(ErrorMessage = "El paciente debe tener nombre")]
        public string? PatientName { get; set; }
        [Required(ErrorMessage = "El paciente debe tener apellidos")]
        public string? PatientLastName { get; set; }
        [Required(ErrorMessage = "El paciente debe tener un género")]
        public string? PatientGender { get; set; }
        public string? PatientHomePhone { get; set; }
        [Required(ErrorMessage = "El paciente debe tener mínimo un número de teléfono")]
        [StringLength(10, MinimumLength = 10)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "El número de teléfono debe tener 10 dígitos numéricos")]
        public string? PatientMobilePhone { get; set; }
        public string? PatientWorkPhone { get; set; }
        [Required(ErrorMessage = "El paciente debe tener un email válido")]
        public string? PatientEmail { get; set; }
        [Required(ErrorMessage = "El paciente debe tener una fecha de nacimiento")]
        public string? PatientBirthDate { get; set; }
        public bool PatientDeceased { get; set; }
        public string? PatientAddress { get; set; }
        public string? PatientMaritalStatus { get; set; }
        public string? PatientStatus { get; set; }
        public string? PatientPMS { get; set; }
        [Required(ErrorMessage = "El paciente debe tener un número de seguro")]
        public string? PatientSSN { get; set; }
        public string? PatientExtensionRace { get; set; }
        public string? PatientEthnicity { get; set; }
        public string? PatientEmergencyContactPhone { get; set; }
        public string? PatientComunicationLanguagePreference { get; set; }
        public string? PatientGeneralPractitioner { get; set; }  
        public string? PatientGeneralPractitionerExtensionDateLastSeen { get; set; }
        public string? PatientReferralResource { get; set; }


    }
}
