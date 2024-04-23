using FoodShop.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodShop.Persistence.ModelsConfiguration
{
    public class FoodConfiguration : IEntityTypeConfiguration<Food>
    {
        public void Configure(EntityTypeBuilder<Food> builder)
        {
            builder.ToTable("Foods");
            builder.HasKey(f => f.FoodId);
            builder.Property(f => f.Name).IsRequired();
            builder.Property(f => f.Description).IsRequired();
            builder.Property(f => f.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(f => f.AvailableQuantity).IsRequired();

            builder.HasMany(f => f.OrderDetails)
              .WithOne(od => od.Food)
              .HasForeignKey(od => od.FoodId);
        }
    }
}
