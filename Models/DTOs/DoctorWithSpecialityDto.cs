namespace HealthBridgeAPI.Models.DTOs
{
    public class DoctorWithSpecialityDto
    {
        public long Id { get; set; }
        public bool Active { get; set; }
        public string Family { get; set; } = string.Empty;
        public string Given { get; set; } = string.Empty;

        // Aquí se guardará la lista de especialidades en formato "Spec1, Spec2, Spec3"
        public string? Specialty { get; set; }
    }
}
