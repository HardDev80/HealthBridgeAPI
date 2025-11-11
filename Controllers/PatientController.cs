using HealthBridgeAPI.Services.Implementations;
using HealthBridgeAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HealthBridgeAPI.Controllers
{
    [ApiController]
    [Route("api/patient")]
    public class PatientController : ControllerBase
    {
        private readonly IModMedPatientService _patientService;

        public PatientController(IModMedPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet("getById/{id}")]

        public async Task<IActionResult> GetPatientById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("El ID es obligatorio");
            }
            var result = await _patientService.GetPatientByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("getAllPatientList")]

        public async Task<IActionResult> GetAllPatients()
        {
            var list = await _patientService.GetAllPatientsAsync();
            return Ok(list);
        }

    }
}
