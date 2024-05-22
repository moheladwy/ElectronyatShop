using ElectronyatShop.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ElectronyatShop.Interfaces;

namespace ElectronyatShop.Data
{
    public class SqliteDbContext : IdentityDbContext<ApplicationUser>, IDbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Order> Orders { get; set; }

        public SqliteDbContext(DbContextOptions<SqliteDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}