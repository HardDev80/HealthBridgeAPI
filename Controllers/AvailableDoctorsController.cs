using HealthBridgeAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HealthBridgeAPI.Controllers
{
    [ApiController]
    [Route("api/doctors")]
    public class AvailableDoctorsController: ControllerBase
    {
        private readonly IAvailableDoctorsService _service;

        public AvailableDoctorsController(IAvailableDoctorsService service)
        {
                _service = service;
        }

        [HttpGet("availableDoctorsList")]

        public async Task<ActionResult> GetAvailableDoctors()
        {
            var doctors = await _service.GetAvailableDoctorsAsync();
            return Ok(doctors);
        }
    }
}
