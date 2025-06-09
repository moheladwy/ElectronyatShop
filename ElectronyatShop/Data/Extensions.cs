using System.Security.Claims;
using ElectronyatShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ElectronyatShop.Data;

public static class Extensions
{
    public static void AddDatabaseToServices(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("PostgresConnection")!;
        
        builder.Services.AddDbContext<ElectronyatShopDbContext>(
            options =>
            {
                options.UseNpgsql(connectionString, o =>
                {
                    o.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorCodesToAdd: null
                    );
                });
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            }
        );
    }
    
    public static void AddDatabaseIdentityToServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddDefaultIdentity<ApplicationUser>(options => 
                options.SignIn.RequireConfirmedAccount = false)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ElectronyatShopDbContext>()
            .AddSignInManager<SignInManager<ApplicationUser>>();
    }

    public static void AddUserRolesToDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("AdminRole", op =>
                op.RequireClaim("Admin", "Admin"))
            .AddPolicy("CustomerRole", op =>
                op.RequireClaim("Customer", "Customer"));
    }
    
    public static async Task EnableMigrationsOnStartup(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ElectronyatShopDbContext>();
        await db.Database.MigrateAsync();
    }
    
    public static async Task AddAdminToDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        const string role = "AdminRole";
        const string email = "admin@admin.com";

        var adminClaim = new Claim("Admin", "Admin");
        var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>()
                          ?? throw new NotSupportedException("roleManager null");

        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var admin = await userManager.FindByEmailAsync(email);
        if (admin is not null) return;

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