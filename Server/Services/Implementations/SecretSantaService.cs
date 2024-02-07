using ADIRA.Server.Services.Interfaces;
using ADIRA.Shared.BusinessDataObjects;
using ADIRA.Shared.Utilities;
using ADIRA.Shared.Utilities.Enums;
using Newtonsoft.Json;

namespace ADIRA.Server.Services.Implementations
{
    public class SecretSantaService : ISecretSantaService
    {
        private readonly SendNotification _notification;
        private readonly IConfiguration _configuration;

        public SecretSantaService(SendNotification notification,
                                  IConfiguration configuration) 
        { 
            _notification = notification;
            _configuration = configuration;
        }

        public async Task<int> AllotSecretSanta(string employeeId, int entityId, string location)
        {
            List<SecretSantaData> secretSantaEmployees = new();
            string jsonData, folderPath = _configuration["FileStoragePath"];

            if (string.IsNullOrWhiteSpace(folderPath)) return 0;

            string empDataFilePath = Path.Combine(folderPath, "EmployeeData.json");

            if (File.Exists(empDataFilePath))
            {
                jsonData = await File.ReadAllTextAsync(empDataFilePath);
            }
            else
            {
                return 0;
            }

            List<Employee> employees = JsonConvert.DeserializeObject<List<Employee>>(jsonData);
            List<int> allotedIndices = new();

            if (employees != null && employees.Any())
            {
                foreach(var item in employees)
                {
                    int randNum = GetUniqueRandomNumberForSecretSanta(0, employees.Count, allotedIndices, item.Entity, employees);
                    allotedIndices.Add(randNum);
                    var allotedSecretSanta = employees[randNum];

                    SecretSantaData secretSantaData = new()
                    {
                        ID = item.ID,
                        Entity = item.Entity,
                        Name = item.Name,
                        Email = item.Email,
                        Department = item.Department,
                        SecretSantaEmployeeId = allotedSecretSanta.ID,
                        SecretSantaEmployeeEntity = allotedSecretSanta.Entity,
                        SecretSantaEmployeeName = allotedSecretSanta.Name,
                        SecretSantaEmployeeEmail = allotedSecretSanta.Email,
                        SecretSantaEmployeeDepartment = allotedSecretSanta.Department,
                        EmailSent = false,
                    };

                    secretSantaEmployees.Add(secretSantaData);
                }

                string santaDataFilePath = Path.Combine(folderPath, "SecretSantaData.json");
                File.WriteAllText(santaDataFilePath, JsonConvert.SerializeObject(secretSantaEmployees));
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public async Task<int> SendSecretSantaMails()
        {
            string jsonData, folderPath = _configuration["FileStoragePath"];

            if (string.IsNullOrWhiteSpace(folderPath)) return 0;

            string filePath = Path.Combine(folderPath, "SecretSantaData.json");

            if (File.Exists(filePath))
            {
                jsonData = await File.ReadAllTextAsync(filePath);
            }
            else
            {
                return 0;
            }

            List<SecretSantaData> secretSantaEmployees = JsonConvert.DeserializeObject<List<SecretSantaData>>(jsonData);

            if (secretSantaEmployees != null && secretSantaEmployees.Any())
            {
                if (secretSantaEmployees.All(x => x.EmailSent)) return 2;

                foreach (var emp in secretSantaEmployees.Where(x => !x.EmailSent).Take(25))
                {
                    if (emp.EmailSent) continue;

                    EmailMessageData emailMessageData = new()
                    {
                        MessageID = Guid.NewGuid().ToString(),
                        HighPriorityFlag = true,
                        Subject = "🎁 Your Secret Santa Details Revealed 🎁",
                        BodyType = MailBodyType.Html,
                        Body = File.ReadAllText("Files\\SecretSantaMailer.txt")
                                   .Replace("[EmpName]", $"{emp.Name}")
                                   .Replace("[SantaName]", $"{emp.SecretSantaEmployeeName} ({emp.ID})")
                                   .Replace("[SantaDeptName]", $"{emp.SecretSantaEmployeeDepartment}")
                                   .Replace("[SantaEntityName]", $"{emp.SecretSantaEmployeeEntity}"),
                        To = new List<string> { emp.Email },
                    };

                    await _notification.SendSMTPEmail(emailMessageData);
                    emp.EmailSent = true;
                    File.WriteAllText(filePath, JsonConvert.SerializeObject(secretSantaEmployees));
                }
            }

            return 1;
        }

        private int GetUniqueRandomNumberForSecretSanta(int min, int max, IEnumerable<int> existingNumbers, string entity, List<Employee> employees)
        {
            int randNum = Util.GetRandomNumber(min, max);

            if (existingNumbers.Contains(randNum))// || !employees[randNum].Entity.Equals(entity))
            {
                randNum = GetUniqueRandomNumberForSecretSanta(min, max, existingNumbers, entity, employees);
            }
            
            return randNum;
        }
    }
}
