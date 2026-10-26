using Ariza.Models;
using Microsoft.EntityFrameworkCore;

namespace Ariza
{
    public class Context:DbContext 
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<UserRole> UserRoles { get; set; } = null!;
        public DbSet<Talep> Taleps { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(user => user.Email)
                .IsUnique();

            modelBuilder.Entity<Role>()
                .HasIndex(role => role.Rol)
                .IsUnique();

            modelBuilder.Entity<UserRole>()
                .HasIndex(userRole => new { userRole.UserId, userRole.RoleId })
                .IsUnique();

            modelBuilder.Entity<Talep>()
                .HasOne(a => a.User)
                .WithMany(u => u.Taleps)
                .HasForeignKey(a => a.UserId);
            base.OnModelCreating(modelBuilder);
        }
    }
}
