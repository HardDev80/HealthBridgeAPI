using HealthBridgeAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HealthBridgeAPI.Controllers
{
    [Controller]
    [Route("api/practitioner")]
    public class PractitionerController : ControllerBase
    {
        private readonly IModMedPractitionerService _modMedPractitionerService;

        public PractitionerController(IModMedPractitionerService modMedPractitionerService)
        {
            _modMedPractitionerService = modMedPractitionerService;
        }

        [HttpGet("getAllPractitionerList")]

        public async Task<IActionResult> AllPractitionerList()
        {
            var list = await _modMedPractitionerService.GetAllPractitionerList();

            return Ok(list);
        }

        [HttpGet("allDailyPractitionerList")]
        public async Task<IActionResult> GetAllPractitioner()
        {
            var result = await _modMedPractitionerService.GetAllPractitionersPaginatedAsync();
            return Ok(result);
        }

        ///ENDPOINT SOLO DE PRUEBA PARA FUNCION DE ACTUALIZACION DIARIA DE LISTA BDE PRACTITIONERS ACTIVOS QUE 
        ///SE ACTIVA TODOS LOS DIAS A LAS 12 AM HORA DE ESTADOS UNIDOS

        [HttpPost("dailyActualizationPractitionerList")]

        public async Task<IActionResult> RunDailyPractitionerList()
        {
            await _modMedPractitionerService.SyncPractitionersToDatabaseAsync();
            return Ok("Ejecución diaria exitosa");
        }

        [HttpGet("getAllDoctorsWithSpeciality")]
        public async Task<IActionResult> GetAllDoctorsWithSpeciality()
        {
            var result = await _modMedPractitionerService.GetAllDoctorsWithSpecialityAsync();
            return Ok(result);
        }

    }
}
