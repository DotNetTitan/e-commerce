using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Persistence.EntityConfigurations
{
    internal sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            // Configure primary key
            builder.HasKey(p => p.OrderItemId);

            // Configure properties
            builder.Property(p => p.UnitPrice)
                .HasColumnType("decimal(18, 2)");

            builder.Property(p => p.Quantity)
                .IsRequired();

            // Configure relationships
            builder.HasOne(p => p.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Product)
                .WithMany()
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(oi => new { oi.OrderId, oi.ProductId });
        }
    }
}