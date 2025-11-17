using HealthBridgeAPI.Models.DTOs;
using HealthBridgeAPI.Models.Entities;
using HealthBridgeAPI.Services.Implementations;
using HealthBridgeAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
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

        /*[HttpGet("getPatientByBirthdateAndEmail")]

        public async Task<IActionResult> GetPatientByEmailAndBirthDate()
        {
            var exist = await _patientService.GetPatientByEmailAndBirthDateAsync();
            return Ok(exist);
        }*/

        [HttpPost("patientConfirmation")]
        public async Task<IActionResult> VerifyPatient([FromBody] PatientConfirmIfExistDto request)
        {
            if (request == null)
                return BadRequest("La petición no puede ser nula o vacía !!");

            if (string.IsNullOrWhiteSpace(request.PatientEmail))
                return BadRequest("El email es requerido !!.");

            if (!DateTime.TryParse(request.PatientBirthDate, out var birthDate))
                return BadRequest("Formato invalido, use yyyy-MM-dd.");

            var result = await _patientService.FindPatientByEmailAndBirthDateAsync(
                request.PatientEmail,
                birthDate
            );

            if (result == null)
            {
                return NotFound(new
                {
                    message = "Paciente no registrado en ModMed."
                });
            }

            return Ok(result);
        }

        [HttpPost("register")]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterPatient([FromBody] Patient patient)
        {
            if (patient == null)
                return BadRequest("La información del paciente es obligatoria.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = await _patientService.RegisterPatientAsync(patient);

                return CreatedAtAction(nameof(GetPatientById),
                    new { id = created.PatientId },
                    new
                    {
                        created.PatientId,
                        created.PatientFirstName,
                        created.PatientLastName,
                        created.PatientEmail,
                        created.PatientBirthDate,
                        created.PatientPMS // id ModMed
                    });
            }
            catch (ValidationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

    }
}
