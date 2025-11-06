using HealthBridgeAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HealthBridgeAPI.Controllers
{
    [ApiController]
    [Route("api/patient")]
    public class PatientController: ControllerBase
    {
        private readonly ModMedService modMedService;
        public PatientController(ModMedService modMedService)
        {
            this.modMedService = modMedService;
        }

        [HttpGet("getPatientList")]
        public async Task<IActionResult> GetAllPatientList()
        {
            var data = await modMedService.GetAsync("patients");
            return Ok(data);
            
        }

        [HttpGet("getPatientById")]
        public IActionResult GetPatientById()
        {
            return Ok("Obtención de paciente por ID exitosa desde PatientController");
        }

        [HttpPost("createPatient")]
        public async Task<IActionResult> CreatePatient()
        {
            return Ok("Creación de paciente exitosa desde PatientController");
        }
    }
}
