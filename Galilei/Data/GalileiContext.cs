using Galilei.Models;
using Microsoft.EntityFrameworkCore;

namespace Galilei.Data
{
    public class GalileiContext : DbContext
    {
        public GalileiContext(DbContextOptions<GalileiContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .ToTable("Users");

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
