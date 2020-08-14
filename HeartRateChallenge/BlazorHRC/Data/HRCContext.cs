using BlazorHRC.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorHRC.Data
{
    public class HRCContext : DbContext
    {
        public HRCContext(DbContextOptions<HRCContext> options) : base(options)
        {
        }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
