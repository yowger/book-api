using BookStore.Api.Entities;
using Microsoft.AspNetCore.Identity;

namespace BookStore.Api.Data.seed;

public static class IdentityDataSeeder
{
    public static async Task SeedRoles(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        String[] roleNames = { "ADMIN", "USER" };

        foreach (var role in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(role);

            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    public static async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
        var adminEmail = "admin@bookstore.com";
        var adminPassword = "Admin@123";

        var existingUser = await userManager.FindByEmailAsync(adminEmail);

        if (existingUser == null)
        {
            var admin = new AppUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(admin, adminPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "ADMIN");
            }
        }
    }
}