using ADIRA.Server.Models;
using ADIRA.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ADIRA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecretSantaController : ControllerBase
    {
        private readonly ISecretSantaService _secretSantaService;
        private readonly AdiraContext _context;


        public SecretSantaController(ISecretSantaService secretSantaService, AdiraContext context) 
        {
            _context = context;
            _secretSantaService = secretSantaService;
        }



        [HttpPost("allocate")]
        [Route("allocate/{entId:int}/{location}")]

        // Add the [Authorize] attribute to ensure the request is made by an authenticated user
        public async Task<IActionResult> AllotSecretSanta(int entId, string location)
        {
            
            // Retrieve the JWT token from the Authorization header
            var jwtToken = HttpContext.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(jwtToken))
            {
                return BadRequest("Token is missing or invalid.");
            }

            // Validate and decode the JWT token
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(jwtToken) as JwtSecurityToken;

            if (jsonToken == null)
            {
                return BadRequest("Invalid token format.");
            }



            // Access user claims from the decoded token
            var claims = jsonToken.Claims;

           
            // Access other claims if needed
            string userName = claims.FirstOrDefault(c => c.Type == "unique_name")?.Value;


            // Access other claims if needed
            string role = claims.FirstOrDefault(c => c.Type == "role")?.Value;

            string EmployeeId = "";
            int roleId = _context.Roles.Where(d=>d.Name==role).Select(d=>d.RoleId).FirstOrDefault();
           
               
                if (roleId != null)
                {
                    var employee = _context.Employees.FirstOrDefault(e => e.Name == userName && e.RoleId == roleId);
                    if (employee != null)
                    {
                        EmployeeId = employee.EmployeeId;
                    }
                }
            

            var result = await _secretSantaService.AllotSecretSanta(EmployeeId,entId,location);

            if (result == 1)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Unknown Error Occurred");
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
