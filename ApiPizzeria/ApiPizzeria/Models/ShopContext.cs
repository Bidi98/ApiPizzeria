using Microsoft.EntityFrameworkCore;

namespace ApiPizzeria.Models
{
    public class ShopContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public string DbPath { get; set; }
        public ShopContext()
        {
            
            DbPath = System.IO.Path.Join(Environment.CurrentDirectory, "shop.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={DbPath}");


        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId);

            modelBuilder.Entity<Product>()
                .HasMany( u => u.OrderProducts)
                .WithOne(u => u.Product)
                .HasForeignKey( u => u.ProductId);

            modelBuilder.Entity<Order>()
                .HasMany( u => u.OrderProducts)
                .WithOne( u => u.Order)
                .HasForeignKey( u => u.OrderId);
        }

    }
}
