using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> op):base(op) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Address> Addresses { get; set; }


    }
}
