using Ariza.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ariza.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(
        Context context,
        IPasswordHasher<User> passwordHasher,
        IConfiguration configuration)
    {
        var roles = new[] { RoleNames.Admin, RoleNames.User };
        foreach (var roleName in roles)
        {
            if (!await context.Roles.AnyAsync(role => role.Rol == roleName))
            {
                context.Roles.Add(new Role { Rol = roleName });
            }
        }

        await context.SaveChangesAsync();

        var adminEmail = configuration["BootstrapAdmin:Email"]?.Trim().ToLowerInvariant();
        var adminPassword = configuration["BootstrapAdmin:Password"];
        if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
        {
            return;
        }

        var admin = await context.Users
            .Include(user => user.UserRoles)
            .SingleOrDefaultAsync(user => user.Email == adminEmail);
        if (admin is null)
        {
            admin = new User
            {
                Ad = "Sistem",
                Soyad = "Yöneticisi",
                Email = adminEmail
            };
            admin.Sifre = passwordHasher.HashPassword(admin, adminPassword);
            context.Users.Add(admin);
            await context.SaveChangesAsync();
        }

        var adminRole = await context.Roles.SingleAsync(role => role.Rol == RoleNames.Admin);
        if (!admin.UserRoles.Any(userRole => userRole.RoleId == adminRole.Id))
        {
            context.UserRoles.Add(new UserRole { UserId = admin.Id, RoleId = adminRole.Id });
            await context.SaveChangesAsync();
        }
    }
}
