using System.ComponentModel.DataAnnotations;

namespace HealthBridgeAPI.Models.Entities
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        [Required(ErrorMessage = "La cita requiere de un estado")]
        public string? AppointmentStatus { get; set; }

        public string? AppointmentCancellationReason { get; set; }

        [Required(ErrorMessage = "Debe haber un tipo de cita")]
        public string? AppointmentType { get; set; }

        public string? AppointmentReason { get; set; }
        public string? AppointmentDescription { get; set; }
        public string? AppointmentComments { get; set; }

        [Required(ErrorMessage = "La cita debe tener fecha y hora de comienzo")]
        public string? AppointmentStartDateTime { get; set; }

        [Required(ErrorMessage = "La cita debe tener una fecha y hora de finalización")]
        public string? AppointmentEndDateTime { get; set; }

        [Required(ErrorMessage = "La cita debe tener definido el tiempo de duración en minutos")]
        public string? AppointmentMinutesDuration { get; set; }

        public string? AppointmentDateTimeCreated { get; set; }

        [Required(ErrorMessage = "La cita debe tener el número identificador del paciente")]
        public string? AppointmentParticipantPatientId { get; set; }

        [Required(ErrorMessage = "La cita debe tener el número identificador del doctor")]
        public string? AppointmentParticipantPractitionerId { get; set; }

        [Required(ErrorMessage = "La cita debe tener el identificador del lugar de atención")]
        public int LocationId { get; set; }
        public Location? Locations { get; set; }

        // FK y navegación: convenciones de EF Core funcionan sin atributos
        [Required]
        public int PatientId { get; set; }

        // navegación simple (sin atributos)
        public Patient? Patient { get; set; }
    }
}
