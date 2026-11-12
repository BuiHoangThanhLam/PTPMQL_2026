using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Data;

public static class DbInitializer
{
    private static readonly string[] DefaultRoles = ["Admin", "User"];

    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();

        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // Tạo sẵn các role để các chức năng phân quyền có thể sử dụng ngay.
        foreach (var role in DefaultRoles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(role));
                if (!result.Succeeded)
                {
                    var errors = string.Join("; ", result.Errors.Select(error => error.Description));
                    throw new InvalidOperationException($"Không thể tạo role '{role}': {errors}");
                }
            }
        }
    }
}
