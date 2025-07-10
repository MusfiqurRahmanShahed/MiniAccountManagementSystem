using AccountManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountManagementSystem.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected ApplicationDbContext()
        {
        }

        public DbSet<AccountView> ChartOfAccount { get; set; } = null!;
    }
}
