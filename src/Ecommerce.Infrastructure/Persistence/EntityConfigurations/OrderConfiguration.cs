using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Persistence.EntityConfigurations
{
    internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.OrderId);

            builder.Property<int>("ClusterId")
                .UseIdentityColumn();

            builder.HasIndex("ClusterId").IsClustered();

            builder.Property(o => o.OrderDate).IsRequired();

            builder.Property(p => p.TotalAmount)
                .HasColumnType("decimal(18, 2)");

            builder.HasIndex(o => o.CustomerId);

            builder.HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.OwnsOne(c => c.ShippingAddress, address =>
            {
                address.Property(a => a.Building).HasColumnName("ShippingAddressBuilding");
                address.Property(a => a.Street).HasColumnName("ShippingAddressStreet");
                address.Property(a => a.PostalCode).HasColumnName("ShippingAddressPostalCode");
                address.Property(a => a.City).HasColumnName("ShippingAddressCity");
                address.Property(a => a.State).HasColumnName("ShippingAddressState");
                address.Property(a => a.Country).HasColumnName("ShippingAddressCountry");
            });

            builder.HasIndex(o => o.OrderDate);
        }
    }
}