using Microsoft.EntityFrameworkCore;

namespace TestWebApplication.Models
{
    public class VocationContext : DbContext
    {
        public VocationContext(DbContextOptions<VocationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Vocation> Vocations { get; set; }
    }
}