using Microsoft.EntityFrameworkCore;

namespace HRCCore.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Competitor> Competitors { get; set; }
    }
}