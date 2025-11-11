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
    }
}
