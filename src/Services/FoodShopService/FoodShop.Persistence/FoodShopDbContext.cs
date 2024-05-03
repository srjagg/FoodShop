using FoodShop.Model.Models;
using FoodShop.Persistence.ModelsConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FoodShop.Persistence
{
    public class FoodShopDbContext : DbContext
    {
        private readonly IConfiguration _config;

        public FoodShopDbContext(IConfiguration config, DbContextOptions<FoodShopDbContext> options) : base(options)
        {
            _config = config;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _config.GetConnectionString("DBCnxSqlServer");

                if (string.IsNullOrEmpty(connectionString))
                    throw new Exception("Connection string not found");

                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new FoodConfiguration());
            modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
        }
    }
}
