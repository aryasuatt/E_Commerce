using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SanalMarketAPI.Models;

namespace SanalMarketAPI.Data
{
    // Use ApplicationUser for custom identity user model
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSet'ler
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItems> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // CartItem ile User arasındaki ilişkiyi tanımla
            modelBuilder.Entity<CartItems>()
                .HasOne(c => c.Customer)
                .WithMany(u => u.CartItems)
                .HasForeignKey(c => c.CustomerId)
                .OnDelete(DeleteBehavior.Restrict); // Veya DeleteBehavior.SetNull


            modelBuilder.Entity<CartItems>()
                .HasOne(c => c.Customer)
                .WithMany(u => u.CartItems)
                .HasForeignKey(c => c.CustomerId);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)"); // 18 toplam basamak, 2 ondalık basamak

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");


            // OrderItem ile Order arasındaki ilişkiyi tanımla

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // Order silinirse OrderItems da silinsin

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // Product silinirse, OrderItems kalmaya devam etsin



        }

    }

}
