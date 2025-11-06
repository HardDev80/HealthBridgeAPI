using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop.Infrastructure;

namespace HealthBridgeAPI.Controllers
{
    [ApiController]
    [Route("api/appointment")]
    public class AppointmentController: ControllerBase
    {
        [HttpGet("allAppointmentList")]
        public IActionResult GetAllAppointmentList()
        {
           
            return Ok("This endpoint will return all appointments.");
        }

        [HttpPost("appointmentCreate")]
        public IActionResult CreateAppointment()
        {
            return Ok("This endpoint will create a new appointment.");
        }
    }
}
