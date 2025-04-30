using Microsoft.EntityFrameworkCore;

namespace TheLab.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Reviews> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reviews>()
                .HasOne(r => r.User)  
                .WithMany(u => u.Reviews)  
                .HasForeignKey(r => r.UserId); 

            base.OnModelCreating(modelBuilder);
        }
    }

}
