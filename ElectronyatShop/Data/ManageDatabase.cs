using System.Security.Claims;
using ElectronyatShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ElectronyatShop.Data
{
    public class ManageDatabase
    {
        public async Task AddAdminToDB(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<SqliteDbContext>();
                db.Database.Migrate();

                const string role = "AdminRole";
                const string email = "admin@admin.com";

                var adminClaim = new Claim("Admin", "Admin");
                var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>()
                ?? throw new NotSupportedException("roleManager null");

                IdentityResult result;
                if (!await roleManager.RoleExistsAsync(role))
                {
                    result = await roleManager.CreateAsync(new IdentityRole(role));
                }

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var admin = await userManager.FindByEmailAsync(email);
                if (admin == null)
                {
                    admin = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true,
                        FirstName = "Mohamed",
                        LastName = "Al-Adawy",
                        Address = "Giza, Egypt",
                        PhoneNumber = "01120664373"
                    };
                    await userManager.CreateAsync(admin, "Admin@123");
                    await userManager.AddClaimAsync(admin, adminClaim);
                    await userManager.AddToRoleAsync(admin, role);
                }
            }
        }
    }
}