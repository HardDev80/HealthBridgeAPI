using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthBridgeAPI.Models.Entities
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "El paciente debe tener nombre")]
        public string? PatientFirstName { get; set; }

        [Required(ErrorMessage = "El paciente debe tener apellidos")]
        public string? PatientLastName { get; set; }

        public string? PatientGender { get; set; }

        [Required(ErrorMessage = "El paciente debe tener mínimo un número de teléfono")]
        [StringLength(10, MinimumLength = 10)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "El número de teléfono debe tener 10 dígitos numéricos")]
        public string? PatientMobilePhone { get; set; }

        public string? PatientHomePhone { get; set; }
        public string? PatientWorkPhone { get; set; }

        [Required(ErrorMessage = "El paciente debe tener un email válido")]
        [EmailAddress]
        public string? PatientEmail { get; set; }

        [Required(ErrorMessage = "El paciente debe tener una fecha de nacimiento")]
        public DateTime? PatientBirthDate { get; set; }
        public string? PatientCity { get; set; }
        public string? PatientState { get; set; }
        public string? PatientPostalCode { get; set; }
        public string? PatientCountry { get; set; }


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

        // Inicializar la colección para evitar NRE en tiempo de ejecución
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
