using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Domain.Entities;
using System.Reflection.Emit;

namespace Ecommerce.Infrastructure.EntityConfigurations
{
    internal sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Property(p => p.Amount)
                .HasColumnType("decimal(18, 2)");
        }
    }
}