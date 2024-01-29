using ElectronyatShop.Data;
using ElectronyatShop.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronyatShop;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? 
                               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
            
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddAuthorization(option =>
            option.AddPolicy("AdminRole", op => op.RequireClaim("Admin", "Admin")));

        builder.Services.AddAuthorization(option =>
            option.AddPolicy("CustomerRole", op => op.RequireClaim("Customer", "Customer")));
        
        builder.Services.AddControllers();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
        }
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        
        app.Run();
    }
}