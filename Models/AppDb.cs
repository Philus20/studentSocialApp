using Microsoft.EntityFrameworkCore;

namespace FinalProject.Models
{
    public class AppDb:DbContext
    {
        public DbSet<Student>  Students { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Post> Posts { get; set; }

        public DbSet<register> Registers { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet <User> Users { get;set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=stuNet;User Id=sa;Password=dockerStrongPwd123;TrustServerCertificate=True;");
        }
    }
}
