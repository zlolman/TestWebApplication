using Microsoft.EntityFrameworkCore;
using TestWebApplication.Models;
using TestWebApplication.Services;

namespace TestWebApplication.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.Migrate();
            AddVocationCheckService.DbInit(this);
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Vocation> Vocations { get; set; }
    }
}