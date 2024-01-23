using System;
using System.Linq;
using ADIRA.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace ADIRA.Server.Authentication
{
    public class UserAccountService
    {
        private readonly AdiraContext dbContext;

        public UserAccountService(AdiraContext context)
        {
            dbContext = context;
        }

        public UserAccount? GetUserAccountByUserName(string username)
        {
            var query = dbContext.Users
                .Where(u => u.Username == username)
                .Join(
                    dbContext.Employees,
                    u => u.UserId,
                    emp => emp.UserId,
                    (u, emp) => new { User = u, Employee = emp }
                )
                .GroupJoin(
                    dbContext.Roles,
                    e => e.Employee.RoleId,
                    role => role.RoleId,
                    (e, roles) => new { UserEmployee = e, Roles = roles }
                )
                .SelectMany(
                    x => x.Roles.DefaultIfEmpty(),
                    (x, r) => new
                    {
                       // EmployeeFullName = x.UserEmployee.Employee.FirstName + " " + x.UserEmployee.Employee.MiddleName + " " + x.UserEmployee.Employee.LastName,
                        Username = x.UserEmployee.User.Username,
                        Password = x.UserEmployee.User.Password,
                        RoleName = r != null ? r.Name : null
                    }
                );

            var singleRecord = query.FirstOrDefault();

            if (singleRecord != null)
            {
                UserAccount userAccount = new UserAccount()
                {
                    UserName = singleRecord.Username,
                    Password = singleRecord.Password,
                   // FullName = singleRecord.EmployeeFullName,
                    Role = singleRecord.RoleName
                };

                return userAccount;
            }

            return null;
        }

        public bool AuthenticateUser(string username, string password)
        {
            UserAccount? userAccount = GetUserAccountByUserName(username);

            if (userAccount != null && userAccount.Password == password)
            {
                ResetLoginAttempts(username);
                return true;
            }
            else
            {
                IncrementLoginAttempts(username);

                if (ShouldBlockUser(username))
                {
                    BlockUser(username);
                    return true;
                }

                return false;
            }
        }

        private void IncrementLoginAttempts(string username)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Username == username);

            if (user != null)
            {
                user.LoginAttempts++;
                dbContext.SaveChanges();
            }
        }

        private bool ShouldBlockUser(string username)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Username == username);
            return user != null && user.LoginAttempts >= 3;
        }

        public void BlockUser(string username)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Username == username);

            if (user != null)
            {
                user.IsBlocked = true;
                user.BlockExpirationDate = DateTime.UtcNow.AddMinutes(1);
                dbContext.SaveChanges();
            }
        }
        public void UnBlockUser(string username)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Username == username);

            if (user != null)
            {
                user.IsBlocked = false;
               // user.BlockExpirationDate = DateTime.UtcNow.AddMinutes(30);
                dbContext.SaveChanges();
            }
        }

        public void ResetLoginAttempts(string username)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Username == username);

            if (user != null)
            {
                user.LoginAttempts = 0;
                if(user.IsBlocked==true)
                {
                    // Check if the user is blocked and the block expiration time has passed
                    if (user.BlockExpirationDate.HasValue && user.BlockExpirationDate.Value <= DateTime.UtcNow)
                    {
                        user.IsBlocked = false;
                        user.BlockExpirationDate = null;
                    }
                }

               

                dbContext.SaveChanges();
            }
        }

    }
}

