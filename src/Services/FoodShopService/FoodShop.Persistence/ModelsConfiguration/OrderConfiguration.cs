using FoodShop.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodShop.Persistence.ModelsConfiguration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(o => o.OrderId);
            builder.Property(o => o.OrderDate).IsRequired();
            builder.Property(o => o.Total).IsRequired().HasColumnType("decimal(18,2)");


            builder.HasMany(o => o.OrderDetails)
               .WithOne(od => od.Order)
               .HasForeignKey(od => od.OrderId);
        }
    }
}
