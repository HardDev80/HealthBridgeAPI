using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HealthBridgeAPI.Models.Entities
{
    public class Location
    {
        [Key]
        public int LocationId { get; set; }
        [Required(ErrorMessage = ("La locación debe tener nombre"))]
        public string? LocationName { get; set; }
    }
}
