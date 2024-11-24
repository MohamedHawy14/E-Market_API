using FirstWebApiProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FirstWebApiProject.Configurations
{
    public class PaymentConfigerations : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Property(p => p.PaymentStatus)
          .HasColumnType("bit") // Store as bit in the database
          .HasDefaultValue(false);
        }
    }
}
