using Microsoft.EntityFrameworkCore;
 
namespace BrightIdeas.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        // //DbSet<Model Name> Table Name
        public DbSet<User> User { get; set; }
        public DbSet<Idea> Idea { get; set; }
        public DbSet<Like> Like { get; set; }
    }
}