using ADIRA.Shared.BusinessDataObjects;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ADIRA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public FileController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("employee/save")]
        public IActionResult SaveEmployeeJsonData([FromBody] IEnumerable<Employee> employeeData)
        {
            if (employeeData == null || !employeeData.Any())
            {
                return BadRequest("No data received.");
            }

            string jsonData = JsonConvert.SerializeObject(employeeData);

            string folderPath = _configuration["FileStoragePath"];

            if (string.IsNullOrWhiteSpace(folderPath)) return BadRequest("Filepath not defined");

            if(!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, "EmployeeData.json");

            System.IO.File.WriteAllText(filePath, jsonData);

            return Ok("File saved successfully.");
        }



        [HttpGet("employee/read")]
        public IActionResult ReadEmployeeJsonData()
        {
            string folderPath = _configuration["FileStoragePath"];

            if (string.IsNullOrWhiteSpace(folderPath)) return BadRequest("Filepath not defined");

            string filePath = Path.Combine(folderPath, "EmployeeData.json");
            string jsonData = string.Empty;

            if (System.IO.File.Exists(filePath))
            {
                jsonData = System.IO.File.ReadAllText(filePath);
            }
            else
            {
                return BadRequest("No data to display");
            }

            IEnumerable<Employee> employees = JsonConvert.DeserializeObject<IEnumerable<Employee>>(jsonData);

            if (employees != null && employees.Any())
            {
                return Ok(employees);
            }
            else
            {
                return BadRequest("No data to display");
            }
        }

        [HttpGet("secretsanta/read/{pwd}")]
        public IActionResult ReadSecretSantaJsonData([FromRoute] string pwd)
        {
            string configPwd = _configuration["SantaDataPassword"];

            if (string.IsNullOrWhiteSpace(configPwd)) return BadRequest("Config not found");

            if(string.IsNullOrWhiteSpace(pwd)) return BadRequest("Password not received");

            if (pwd.Equals(configPwd))
            {
                string folderPath = _configuration["FileStoragePath"];

                if (string.IsNullOrWhiteSpace(folderPath)) return BadRequest("Filepath not defined");

                string filePath = Path.Combine(folderPath, "SecretSantaData.json");
                string jsonData = string.Empty;

                if (System.IO.File.Exists(filePath))
                {
                    jsonData = System.IO.File.ReadAllText(filePath);
                }
                else
                {
                    return BadRequest("No data to display");
                }

                IEnumerable<SecretSantaData> secretSantaData = JsonConvert.DeserializeObject<IEnumerable<SecretSantaData>>(jsonData);

                if (secretSantaData != null && secretSantaData.Any())
                {
                    return Ok(secretSantaData);
                }
                else
                {
                    return BadRequest("No data to display");
                }
            }
            else
            {
                return BadRequest("Password Mismatch 😏");
            }
        }
    }
}
