using ElectronyatShopWebAPI.Data;
using ElectronyatShopWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronyatShopWebAPI;

internal static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //     .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAdB2C"));
        // builder.Services.AddAuthorization();

        // Add the ConnectionString to the Builder.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? 
                               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        // Add the DbContext to the Builder.
        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
        
        // Add the Identity to the Builder.
        builder.Services.AddDefaultIdentity<ApplicationUser>(options => 
            options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<ApplicationDbContext>();

        // Add the Authorization Policies of the Admin Role to the Builder.
        builder.Services.AddAuthorization(option => option.AddPolicy("AdminRole",
            op => op.RequireClaim("Admin", "Admin")));
        
        // Add the Authorization Policies of the Customer Role to the Builder.
        builder.Services.AddAuthorization(option => option.AddPolicy("CustomerRole", 
            op => op.RequireClaim("Customer", "Customer")));

        
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        
        // Build the Application.
        var app = builder.Build();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        
        app.Run();
    }
}