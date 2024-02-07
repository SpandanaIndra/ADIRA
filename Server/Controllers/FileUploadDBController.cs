using ADIRA.Server.Models;
using ADIRA.Shared.BusinessDataObjects;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace ADIRA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadDBController : ControllerBase
    {
        private readonly AdiraContext context;
        private readonly IConfiguration _configuration;
        public FileUploadDBController(AdiraContext _context, IConfiguration configuration)
        {
            context= _context;
            _configuration= configuration;
        }
        [HttpPost("employee/save")]
        public async Task<IActionResult> SaveEmployeeJsonData([FromBody] IEnumerable<ADIRA.Shared.BusinessDataObjects.Employee> employeeData)
        {

            if (employeeData != null && employeeData.Any())
            {
                try
                {
                    foreach (var rowData in employeeData)
                    {
                        // Check if an employee with the same email exists in the database
                        var existingEmployee = await  context.Employees.FirstOrDefaultAsync(e => e.Email == rowData.Email&&e.EmployeeId==rowData.ID);

                        if (existingEmployee == null)
                        {
                            // Check and insert into EntityL if necessary
                            int entityId = GetOrCreateLookupId(context.EntityLs, "Name", rowData.Entity,rowData.ID);

                            // Check and insert into DepartmentL if necessary
                            int departmentId = GetOrCreateLookupId(context.DepartmentLs, "Name", rowData.Department,rowData.ID);

                            ADIRA.Server.Models.Employee employee = new ADIRA.Server.Models.Employee()
                            {
                                EmployeeId = rowData.ID,
                                Name = rowData.Name,
                                Email = rowData.Email,
                                IsActive = true, // You may set the default value based on your requirements
                                RoleId = 2, // You may set the default value based on your requirements
                                EntityId = entityId,
                                DepartmentId = departmentId
                            };
                            // Employee with this email does not exist, so add it to the database
                            context.Employees.Add(employee);
                           
                        }
                        await context.SaveChangesAsync();
                      
                    }
                    return Ok("File saved successfully.");



                }
                catch (Exception ex)
                {
                    // Handle exceptions, log errors, or return an error message
                    ModelState.AddModelError("", $"Error occurred while saving data: {ex.Message}");
                    return BadRequest();

                }
            }
            else
            {
                return BadRequest("No data received.");

            }


          
        }

        private int GetOrCreateLookupId<TEntity>(DbSet<TEntity> dbSet, string columnName, string value, string id)
     where TEntity : class
        {
            if (string.IsNullOrEmpty(value))
            {
                // If value is null or empty, check by ID
                if (!string.IsNullOrEmpty(id))
                {
                    var existingEntityById = dbSet.Find(int.Parse(id));
                    if (existingEntityById != null)
                    {
                        // Return the existing entity's ID
                        var idProperty = existingEntityById.GetType().GetProperty("Id");
                        var res= (int)idProperty.GetValue(existingEntityById);
                        return res;
                    }
                }
                // Value is null or empty, and ID is also null or empty
                return 0; // or throw an exception, depending on your requirements
            }

            // Check if the value already exists in the lookup table
            var existingEntity = dbSet.FirstOrDefault(e => EF.Property<string>(e, columnName) == value);

            if (existingEntity != null)
            {
                // Return the existing entity's ID
                var idProperty = existingEntity.GetType().GetProperty("Id");
                var res= (int)idProperty.GetValue(existingEntity);
                return res;
            }
            else
            {
                // If the value does not exist, create a new entity and return its ID
                var newEntity = Activator.CreateInstance<TEntity>();
                var nameProperty = newEntity.GetType().GetProperty(columnName);
                nameProperty.SetValue(newEntity, value);

                // Set other properties as needed
                var descriptionProperty = newEntity.GetType().GetProperty("Description");
                descriptionProperty.SetValue(newEntity, "New entry");

                var createdDateProperty = newEntity.GetType().GetProperty("CreatedDate");
                createdDateProperty.SetValue(newEntity, DateTime.Now);

                // Add the new entity to the DbSet and save changes
                dbSet.Add(newEntity);
                context.SaveChanges();

                // Return the new entity's ID
                var idProperty = newEntity.GetType().GetProperty("Id");
                var res= (int)idProperty.GetValue(newEntity);
                return res;
            }
        }



        [HttpGet("employee/read")]
        public IActionResult ReadEmployeeJsonData()
        {
            var employeesWithDetails = context.Employees
                .Include(e => e.Entity)       // Include the Entity navigation property
                .Include(d => d.Department)   // Include the Department navigation property
                .Select(e => new ADIRA.Shared.BusinessDataObjects.Employee
                {
                    ID = e.EmployeeId.ToString(),
                    Name = e.Name,
                    Email = e.Email,
                    Entity = e.Entity.Name,            // Access the Entity name
                    Department = e.Department.Name    // Access the Department name
                })
                .ToList();

            return Ok(employeesWithDetails);
        }


       
        [HttpGet("secretsanta/read/{targetYear:int}")]
        public IActionResult ReadSecretSantaJsonData([FromRoute] int targetYear)
        {
           
               
                List<SecretSantaData> secretSantaDatas = new List<SecretSantaData>();
            if (targetYear!=0)
            {
                var secret = context.SecretSantaData.Where(e => ((DateTime)e.CreatedDate).Year == targetYear).ToList();

          

                if (secret!=null)
                {
                    foreach(var si in  secret)
                    {
                        var emp=context.Employees.FirstOrDefault(s=>s.EmployeeId==si.EmployeeId);
                        var semp= context.Employees.FirstOrDefault(s => s.EmployeeId == si.SecretSantaEmployeeId);
                  
                    SecretSantaData sss=    new SecretSantaData
                    {
                        ID = emp.EmployeeId,
                      //  Entity = entity.Name,
                        Name = emp.Name,
                        Email = emp.Email,
                        //Department = joinedData.department.Name,
                        SecretSantaEmployeeId = semp.EmployeeId,
                       // SecretSantaEmployeeEntity = joinedData.secretSantaData.Employee.Name,
                        SecretSantaEmployeeName = semp.Name,
                        SecretSantaEmployeeEmail = semp.Email,
                       // SecretSantaEmployeeDepartment = joinedData.secretSantaData.Employee.Department.Name,
                        EmailSent = si.EmailSent
                    };
                        secretSantaDatas.Add(sss);
                    }
                }

             
                if (secretSantaDatas != null && secretSantaDatas.Any())
                {
                    return Ok(secretSantaDatas);
                }
                else
                {
                    return BadRequest("No data to display");
                }
            }
            else
            {
                return BadRequest("Year Mismatch 😏");
            }
        }

        [HttpGet("secretsanta/read/{targetYear:int}/{entID:int}/{location}")]
        public IActionResult ReadSecretSantaFilteredData([FromRoute] int targetYear, int entID,string location)
        {


            List<SecretSantaData> secretSantaDatas = new List<SecretSantaData>();
            if (targetYear != 0)
            {
                var secret = context.SecretSantaData.Where(e => ((DateTime)e.CreatedDate).Year == targetYear).ToList();



                if (secret != null)
                {
                    foreach (var si in secret)
                    {
                        var emp = context.Employees.Where(e=>e.EntityId==entID&&e.Location==location).FirstOrDefault(s => s.EmployeeId == si.EmployeeId);
                        var semp = context.Employees.Where(e => e.EntityId == entID && e.Location == location).FirstOrDefault(s => s.EmployeeId == si.SecretSantaEmployeeId);
                        if(emp != null&&semp!=null)
                        {
                            SecretSantaData sss = new SecretSantaData
                            {
                                ID = emp.EmployeeId,
                                //  Entity = entity.Name,
                                Name = emp.Name,
                                Email = emp.Email,
                                //Department = joinedData.department.Name,
                                SecretSantaEmployeeId = semp.EmployeeId,
                                // SecretSantaEmployeeEntity = joinedData.secretSantaData.Employee.Name,
                                SecretSantaEmployeeName = semp.Name,
                                SecretSantaEmployeeEmail = semp.Email,
                                // SecretSantaEmployeeDepartment = joinedData.secretSantaData.Employee.Department.Name,
                                EmailSent = si.EmailSent
                            };
                            secretSantaDatas.Add(sss);
                        }
                       
                    }
                }


                if (secretSantaDatas != null && secretSantaDatas.Any())
                {
                    return Ok(secretSantaDatas);
                }
                else
                {
                    return BadRequest("No data to display");
                }
            }
            else
            {
                return BadRequest("Year Mismatch 😏");
            }
        }


        [HttpGet("entity/read")]
        public IActionResult ReadEmployeeEntity()
        {
            List<Entity> entities = new List<Entity>();
            var employeesWithDetails = context.EntityLs.ToList();

        
            return Ok(employeesWithDetails);

        }
        [HttpGet("department/read")]
        public IActionResult ReadEmployeeDepartment()
        {
            var employeesWithDetails = context.Employees.Select(e => e.Name).ToList();

           

            return Ok(employeesWithDetails);

            
        }
        [HttpGet("createdDate/read")]
        public IActionResult ReadCreatedDate()
        {
            var distinctYears = context.SecretSantaData
                                .Where(x => x.CreatedDate.HasValue)
                                .Select(x => x.CreatedDate.Value.Year)
                                .Distinct()
                                .ToList();



            return Ok(distinctYears);


        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteEmployee(string id)
        {
            var employeeToDelete = context.Employees.FirstOrDefault(e => e.EmployeeId == id);

            if (employeeToDelete != null)
            {
                var secretSantaEmp = context.SecretSantaData.Where(e => e.EmployeeId == id || e.SecretSantaEmployeeId == id).ToList();

                if (secretSantaEmp.Count > 0)
                {
                    context.SecretSantaData.RemoveRange(secretSantaEmp);
                    context.SaveChanges();
                }

                context.Employees.Remove(employeeToDelete);
                context.SaveChanges();
                return Ok();
            }

            return BadRequest("Employee Not Found...!!");
        }


    }
}
