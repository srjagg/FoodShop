using FoodShop.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodShop.Persistence.ModelsConfiguration
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("OrderDetails");
            builder.HasKey(od => od.OrderDetailId);
            builder.Property(od => od.Quantity).IsRequired();
            builder.Property(od => od.UnitPrice).IsRequired().HasColumnType("decimal(18,2)");
        }
    }
}
