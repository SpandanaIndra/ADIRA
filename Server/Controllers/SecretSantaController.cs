using ADIRA.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ADIRA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecretSantaController : ControllerBase
    {
        private readonly ISecretSantaService _secretSantaService;

        public SecretSantaController(ISecretSantaService secretSantaService) 
        {
            _secretSantaService = secretSantaService;
        }

        [HttpPost("allocate")]
        public async Task<IActionResult> AllotSecretSanta()
        {
            var result = await _secretSantaService.AllotSecretSanta();
            if(result == 1)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Unknown Error Occured");
            }
        }

        [HttpPost("sendmails")]
        public async Task<IActionResult> SendSecretSantaMails()
        {
            var result = await _secretSantaService.SendSecretSantaMails();
            if (result == 1)
            {
                return Ok();
            }
            else if(result == 2)
            {
                return BadRequest("All emails have been successfully sent");
            }
            else
            {
                return BadRequest("Unknown Error Occured");
            }
        }
    }
}
