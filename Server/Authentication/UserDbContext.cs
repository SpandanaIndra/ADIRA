using Microsoft.EntityFrameworkCore;

namespace ADIRA.Server.Authentication
{
    public class UserDbContext:DbContext
    {
        public UserDbContext(DbContextOptions options):base(options)
        {
            
        }
        public DbSet<UserAccount> UserAccounts { get; set; }
    }
}
