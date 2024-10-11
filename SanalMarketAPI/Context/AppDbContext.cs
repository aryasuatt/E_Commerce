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
        public DbSet<Category> Categories { get; set; }
        public DbSet<CartItems> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Review> Reviews { get; set; }    
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Shipping> Shippings { get; set; }
        public DbSet<Discount> Discounts { get; set; }



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

            // Review ile Product ve User arasındaki ilişki
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // Product silinirse Reviews de silinir

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Wishlist ile Product ve User arasındaki ilişki
            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.Product)
                .WithMany(p => p.Wishlists)
                .HasForeignKey(w => w.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.User)
                .WithMany(u => u.Wishlists)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Shipping ile Order arasındaki ilişki
            modelBuilder.Entity<Shipping>()
                .HasOne(s => s.Order)
                .WithMany(o => o.Shipping)
                .HasForeignKey(s => s.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Discount ile Product arasındaki çoktan-çoğa ilişki
            modelBuilder.Entity<Discount>()
                .HasMany(d => d.Products)
                .WithMany(p => p.Discounts)
                .UsingEntity(j => j.ToTable("ProductDiscounts"));




        }

    }

}
