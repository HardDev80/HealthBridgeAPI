using HealthBridgeAPI.Services.Implementations;
using HealthBridgeAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop.Infrastructure;

namespace HealthBridgeAPI.Controllers
{
    [ApiController]
    [Route("api/appointment")]
    public class AppointmentController: ControllerBase
    {
        private readonly IModMedAppointmentService _modMedAppointmentService;
        public AppointmentController(IModMedAppointmentService modMedAppointmentService)
        {
            _modMedAppointmentService = modMedAppointmentService;      
        }

        [HttpGet("allAppointmentList")]
        public async Task<IActionResult> AllAppointmentList()
        {
            var data = await _modMedAppointmentService.GetAllAppointmentList();
            return Ok(data);
        }

        [HttpPost("appointmentCreate")]
        public IActionResult CreateAppointment()
        {
            return Ok("This endpoint will create a new appointment.");
        }
    }
}
