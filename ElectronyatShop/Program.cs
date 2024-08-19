using ElectronyatShop.Data;
using ElectronyatShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ElectronyatShop
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            const string connectionStringName = "SqliteConnection";

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString(connectionStringName) ?? throw new InvalidOperationException($"Connection string '{connectionStringName}' not found.");

            builder.Services.AddDbContext<SqliteDbContext>(options =>
                options.UseSqlite(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>().AddEntityFrameworkStores<SqliteDbContext>().
                AddSignInManager<SignInManager<ApplicationUser>>();

            builder.Services.AddAuthorization(option =>
            {
                option.AddPolicy("AdminRole", op => op.RequireClaim("Admin", "Admin"));
                option.AddPolicy("CustomerRole", op => op.RequireClaim("Customer", "Customer"));
            });

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            var app = builder.Build();

            ManageDatabase manageDatabase = new();
            await manageDatabase.AddAdminToDB(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Product}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "Admin",
                pattern: "{controller=Admin}/{action=Index}/{id?}");

            app.MapRazorPages();
            app.Run();
        }
    }
}