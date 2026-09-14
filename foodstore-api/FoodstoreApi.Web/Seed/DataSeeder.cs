using System.Security.Claims;
using FoodstoreApi.Core.Constants;
using FoodstoreApi.Core.Entities;
using FoodstoreApi.Core.Entities.Identity;
using FoodstoreApi.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FoodstoreApi.Web.Seed;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var context = serviceProvider.GetRequiredService<StoreDbContext>();

        // 1. Seed sources (nếu chưa có)
        if (!await context.Sources.AnyAsync())
        {
            context.Sources.AddRange(
                new Source { Name = "Dine In", IsActive = true },
                new Source { Name = "Take Away", IsActive = true },
                new Source { Name = "Delivery", IsActive = true },
                new Source { Name = "Online", IsActive = true }
            );
            await context.SaveChangesAsync();
        }

        // 2. Seed root role (nếu chưa có)
        var rootRole = await roleManager.FindByNameAsync("root");
        if (rootRole == null)
        {
            var now = DateTime.UtcNow;
            rootRole = new ApplicationRole
            {
                Name = "root",
                Description = "Super Administrator — full system privileges",
                DefaultRoute = "/admin",
                IsSystem = true,
                CreatedAt = now,
                UpdatedAt = now
            };
            await roleManager.CreateAsync(rootRole);

            // Gán tất cả permissions
            foreach (var perm in Permissions.All)
            {
                await roleManager.AddClaimAsync(rootRole, new Claim("Permission", perm.Code));
            }
        }

        // 3. Seed customer role (nếu chưa có)
        var customerRole = await roleManager.FindByNameAsync("customer");
        if (customerRole == null)
        {
            var now = DateTime.UtcNow;
            customerRole = new ApplicationRole
            {
                Name = "customer",
                Description = "Default customer",
                DefaultRoute = "/",
                IsSystem = true,
                CreatedAt = now,
                UpdatedAt = now
            };
            await roleManager.CreateAsync(customerRole);
        }

        // 4. Seed root user (nếu chưa có)
        var rootUser = await userManager.FindByNameAsync("root");
        if (rootUser == null)
        {
            var now = DateTime.UtcNow;
            rootUser = new ApplicationUser
            {
                UserName = "root",
                Email = "root@foodstore.vn",
                Name = "Root Administrator",
                CreatedAt = now,
                UpdatedAt = now
            };
            await userManager.CreateAsync(rootUser, "abc123");
            await userManager.AddToRoleAsync(rootUser, "root");
        }

        // 5. Seed root employee (nếu chưa có)
        if (!await context.Employees.AnyAsync(e => e.EmployeeCode == "NV-00001"))
        {
            rootRole ??= await roleManager.FindByNameAsync("root");
            if (rootRole != null)
            {
                var now = DateTime.UtcNow;
                context.Employees.Add(new Employee
                {
                    UserId = rootUser.Id,
                    EmployeeCode = "NV-00001",
                    RoleId = rootRole.Id,
                    HireDate = now,
                    Status = 1,
                    CreatedAt = now,
                    UpdatedAt = now
                });
                await context.SaveChangesAsync();
            }
        }
    }
}
