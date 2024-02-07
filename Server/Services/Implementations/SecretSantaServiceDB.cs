using ADIRA.Server.Models;
using ADIRA.Server.Services.Interfaces;
using ADIRA.Shared.BusinessDataObjects;
using ADIRA.Shared.Utilities.Enums;
using ADIRA.Shared.Utilities;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.InkML;

namespace ADIRA.Server.Services.Implementations
{
    public class SecretSantaServiceDB:ISecretSantaService
    {
        private readonly SendNotification _notification;
       
        private readonly AdiraContext _context;

        public SecretSantaServiceDB(SendNotification notification, AdiraContext context)
       
        {
            _notification = notification;
            _context = context;
           
        }

        public async Task<int> AllotSecretSanta(string employeeId, int entityId, string location)
        {
          

            List<ADIRA.Server.Models.Employee> employees = _context.Employees.Where(e=>e.EntityId==entityId&&e.Location==location).ToList();
            List<int> allotedIndices = new();

            if (employees != null && employees.Any())
            {
                foreach (var item in employees)
                {
                    int randNum = GetUniqueRandomNumberForSecretSanta(0, employees.Count, allotedIndices, /*item.Entity,*/"", employees);
                    allotedIndices.Add(randNum);
                    var allotedSecretSanta = employees[randNum];

                    SecretSantaDatum secretSantaData = new()
                    {
                       

                        EmployeeId = item.EmployeeId,
                        SecretSantaEmployeeId = allotedSecretSanta.EmployeeId,
                        CreatedBy = employeeId,
                        CreatedDate = DateTime.Now,
                        EmailSent = false





                    };
                    var existingEmployee = await _context.SecretSantaData.FirstOrDefaultAsync(e => e.EmployeeId == secretSantaData.EmployeeId);
                    if (existingEmployee == null)
                    {
                        _context.SecretSantaData.Add(secretSantaData);
                        _context.SaveChanges();
                    }


                }

               
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public async Task<int> SendSecretSantaMails()
        {
           int targetYear = DateTime.Now.Year;
            List<SecretSantaDatum> secretSantaEmployees = _context.SecretSantaData.Where(e => ((DateTime)e.CreatedDate).Year == targetYear).ToList();//JsonConvert.DeserializeObject<List<SecretSantaData>>(jsonData);

            if (secretSantaEmployees != null && secretSantaEmployees.Any())
            {
                if (secretSantaEmployees.All(x => x.EmailSent)) return 2;

                foreach (var emp in secretSantaEmployees.Where(x => !x.EmailSent).Take(25))
                {
                    if (emp.EmailSent) continue;
                    var employ = _context.Employees.FirstOrDefault(n => n.EmployeeId == emp.EmployeeId);
                    var secretSanta = _context.Employees
                        .Where(n => n.EmployeeId == emp.SecretSantaEmployeeId)
                        .Include(e => e.Department)  // Include the Department navigation property
                        .Include(e => e.Entity)      // Include the Entity navigation property
                        .FirstOrDefault();
                    EmailMessageData emailMessageData = new()
                    {

                        MessageID = Guid.NewGuid().ToString(),
                        HighPriorityFlag = true,
                        Subject = "🎁 Your Secret Santa Details Revealed 🎁",
                        BodyType = MailBodyType.Html,
                        Body = File.ReadAllText("Files\\SecretSantaMailer.txt")
                                   .Replace("[EmpName]", $"{employ.Name}")
                               .Replace("[SantaName]", $"{secretSanta.Name} ({secretSanta.EmployeeId})")
                                   .Replace("[SantaDeptName]", $"{secretSanta.Department.Name}")
                                   .Replace("[SantaEntityName]", $"{secretSanta.Entity.Name}"),
                        To = new List<string> { employ.Email },
                    };

                    await _notification.SendSMTPEmail(emailMessageData);
                    emp.EmailSent = true;
                    _context.SaveChanges();
                    
                }
            }

            return 1;
        }

        private int GetUniqueRandomNumberForSecretSanta(int min, int max, IEnumerable<int> existingNumbers, string entity, List<ADIRA.Server.Models.Employee> employees)
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
