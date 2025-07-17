using Microsoft.AspNetCore.Identity;

namespace AccountManagementSystem.Services
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string[] roles = { "Admin", "Accountant", "Viewer" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // ✅ Admin user
            var adminEmail = "admin@local.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newAdmin, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }

            // ✅ Accountant user
            var accEmail = "accountant@local.com";
            var accUser = await userManager.FindByEmailAsync(accEmail);

            if (accUser == null)
            {
                var newAcc = new IdentityUser
                {
                    UserName = accEmail,
                    Email = accEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newAcc, "Account123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAcc, "Accountant");
                }
            }

            // ✅ Viewer user
            var viewEmail = "viewer@local.com";
            var viewUser = await userManager.FindByEmailAsync(viewEmail);

            if (viewUser == null)
            {
                var newViewer = new IdentityUser
                {
                    UserName = viewEmail,
                    Email = viewEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newViewer, "Viewer123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newViewer, "Viewer");
                }
            }
        }
    }
}
