using _2FADemo.Models;
using Microsoft.EntityFrameworkCore;

namespace _2FADemo
{
	// DBContext is responsible for interacting with database data as objects
	public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Database table of users
        public DbSet<User> Users { get; set; }
    }
}
