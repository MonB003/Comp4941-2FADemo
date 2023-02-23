using _2FADemo.Models;
using Microsoft.EntityFrameworkCore;

namespace _2FADemo
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        // Allows program class to use SQL server
        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);
        //}
    }
}
