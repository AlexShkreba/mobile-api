

using Microsoft.EntityFrameworkCore;

namespace MobileAppAPI.Models
{
    public class Context : DbContext
    {
        public DbSet<Users> users { get; set; }
        public DbSet<Categories> categories { get; set; }
        public DbSet<Tasks> tasks { get; set; }
        public DbSet<Tasks_By_Categories> tasks_by_categories { get; set; }
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}