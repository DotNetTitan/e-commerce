using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Infrastructure.EntityConfigurations
{
    internal sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            // Configure primary key
            builder.HasKey(p => p.PaymentId);

            // Configure properties
            builder.Property(p => p.PaymentId)
                .IsRequired();

            builder.Property(p => p.OrderId)
                .IsRequired();

            builder.Property(p => p.Amount)
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            builder.Property(p => p.PaymentDate)
                .IsRequired();

            builder.Property(p => p.PaymentMethod)
                .IsRequired()
                .HasMaxLength(50); // Assuming a max length for the payment method

            // Configure relationships
            builder.HasOne(p => p.Order)
                .WithOne(o => o.Payment) // Change to WithOne for one-to-one relationship
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // Assuming cascade delete behavior
        }
    }
}