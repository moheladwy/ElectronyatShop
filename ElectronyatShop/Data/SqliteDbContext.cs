using ElectronyatShop.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ElectronyatShop.Interfaces;

namespace ElectronyatShop.Data;

public class SqliteDbContext(DbContextOptions<SqliteDbContext> options)
    : IdentityDbContext<ApplicationUser>(options), IDbContext
{
    public DbSet<Product> Products { get; set; }

    public DbSet<CartItem> CartItems { get; set; }

    public DbSet<Cart> Carts { get; set; }

    public DbSet<OrderItem> OrderItems { get; set; }

    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .HasMany(p => p.CartItems)
            .WithOne(ci => ci.Product)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Product>()
            .HasMany(p => p.OrderItems)
            .WithOne(oi => oi.Product)
            .OnDelete(DeleteBehavior.Cascade);
    }
}