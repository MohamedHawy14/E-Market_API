using FirstWebApiProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FirstWebApiProject.Configurations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
           
            builder.Property(o => o.OrderStatus)
           .HasColumnType("bit") // Store as bit in the database
           .HasDefaultValue(false);
        }
    }
}
