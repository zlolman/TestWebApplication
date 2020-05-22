using Microsoft.EntityFrameworkCore;
using TestWebApplication.Models;

namespace TestWebApplication.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
            Database.Migrate();
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Vocation> Vocations { get; set; }
    }
}