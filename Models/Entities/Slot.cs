using System.ComponentModel.DataAnnotations;

namespace HealthBridgeAPI.Models.Entities
{
    public class Slot
    {
        [Key]
        public int SlotId { get; set; }
        [Required(ErrorMessage = ("La cita debe tener un identificador alfanumérico"))]
        public string? SlotModMedIdentifier { get; set; }
        public DateTime SlotSchedule {  get; set; }
        public bool SlotStatus { get; set; }
        public DateTime SlotStartTime { get; set; }
        public DateTime SlotEndTime { get; set; }
    }
}
