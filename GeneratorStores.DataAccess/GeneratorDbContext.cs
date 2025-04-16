using GeneratorStores.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GeneratorStores.DataAccess
{
    public class GeneratorDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryProduct> CategoryProducts { get; set; }
        public DbSet<Review> Reviews { get; set; } // Add Reviews table

        public GeneratorDbContext(DbContextOptions<GeneratorDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Define relationships for Order and ApplicationUser
            builder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany() // If ApplicationUser has a collection of Orders, replace this with .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Define relationships for ProductOrder and Order
            builder.Entity<ProductOrder>()
                .HasOne(po => po.Order)
                .WithMany(o => o.ProductsInOrder)
                .HasForeignKey(po => po.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Define relationships for ProductOrder and Product
            builder.Entity<ProductOrder>()
                .HasOne(po => po.Product)
                .WithMany()
                .HasForeignKey(po => po.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Define relationships for Product and Category through CategoryProduct
            builder.Entity<CategoryProduct>()
                .HasOne(cp => cp.Category)
                .WithMany()
                .HasForeignKey(cp => cp.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CategoryProduct>()
                .HasOne(cp => cp.Product)
                .WithMany()
                .HasForeignKey(cp => cp.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Define relationships for Review
            builder.Entity<Review>()
                .HasOne(r => r.Product)
                .WithMany() // If Product has a collection of Reviews, replace this with .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany() // If ApplicationUser has a collection of Reviews, replace this with .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

