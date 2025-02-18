using Ariza.Models;
using Microsoft.EntityFrameworkCore;

namespace Ariza
{
    public class Context:DbContext 
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=LAPTOP-IEH31APK;database=Ariza;user id=sa;password=1234;TrustServerCertificate=True;");
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Talep> Taleps { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Talep>()
                .HasOne(a => a.User)
                .WithMany(u => u.Taleps)
                .HasForeignKey(a => a.UserId);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>()
       .HasOne(ur => ur.User)
       .WithMany(u => u.UserRoles)
       .HasForeignKey(ur => ur.UserId)
       .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRole>()
       .HasOne(ur => ur.Role)
       .WithMany(r => r.UserRoles)
       .HasForeignKey(ur => ur.RoleId);
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await Users.ToListAsync();  
        }
        public async Task<List<Talep>> GetAllApplicationsAsync()
        {
            return await Taleps.ToListAsync();
        }

        //public async Task<User> GetUserByIdAsync(int id)
        //{
        //    return await Users.FindAsync(id);
        //    //return await Users
        //    // .Include(u => u.UserRoles)
        //    // .ThenInclude(ur => ur.Role)
        //    // .FirstOrDefaultAsync(u => u.Id == id);
        //}
    }
}
