using Microsoft.EntityFrameworkCore;
using TestWebApplication.Models;

namespace TestWebApplication.Data
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