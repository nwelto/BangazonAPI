
    using Microsoft.EntityFrameworkCore;
    using BangazonAPI.Models;

    public class BangazonAPIDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }

        public BangazonAPIDbContext(DbContextOptions<BangazonAPIDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed data for 'Categories'
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Clothing" },
                new Category { Id = 3, Name = "Home & Garden" }
            );

            // Seed data for 'Users'
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, FirebaseUid = "firebase-uid-1", Username = "sellerOne", Email = "sellerone@example.com", IsSeller = true },
                new User { Id = 2, FirebaseUid = "firebase-uid-2", Username = "buyerOne", Email = "buyerone@example.com", IsSeller = false }
            );

            // Seed data for 'Products'
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Description = "A portable computer", Price = 999.99m, Quantity = 10, SellerId = 1, CategoryId = 1 },
                new Product { Id = 2, Name = "T-Shirt", Description = "Cotton t-shirt", Price = 19.99m, Quantity = 50, SellerId = 1, CategoryId = 2 }
            );

            // Seed data for 'Orders'
            modelBuilder.Entity<Order>().HasData(
                new Order { Id = 1, UserId = 2, Status = "Completed", Created = new DateTime(2024, 2, 24) }
            );

            // Seed data for 'OrderProducts'
            modelBuilder.Entity<OrderProduct>().HasData(
                new OrderProduct { Id = 1, ProductId = 1, OrderId = 1 },
                new OrderProduct { Id = 2, ProductId = 2, OrderId = 1 }
            );
        }

    }




