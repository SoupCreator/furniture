using System;
using Microsoft.EntityFrameworkCore;
using myApp.Models;
using myApp.Models;

namespace myApp.Data;
    public class AppDbContext : DbContext
    {
        // Это твоя коллекция товаров, к которой ты будешь обращаться
        public DbSet<Product> Products { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }
        public DbSet<ProductInCart> ProductsInCart { get; set; }
        public DbSet<ServicesInCart> ServicesInCart { get; set; }

        public AppDbContext()
        {
            // Обязательно для корректной работы с DateTime в Postgres
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Подставь свои данные: Host, Database, Username, Password
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=my_database;Username=king;Password=0612");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductInCart>()
                .HasKey(p => new { p.woID, p.productID });

            modelBuilder.Entity<ServicesInCart>()
                .HasKey(s => new { s.woID, s.serviceID });
        }
    }