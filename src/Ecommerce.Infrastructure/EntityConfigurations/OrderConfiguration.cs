using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.EntityConfigurations
{
    internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.OrderId);

            builder.Property(o => o.OrderDate).IsRequired();

            builder.HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.OwnsOne(c => c.ShippingAddress, a =>
            {
                a.Property(aa => aa.Street).HasColumnName("ShippingAddress_Street");
                a.Property(aa => aa.PostalCode).HasColumnName("ShippingAddress_PostalCode");
                a.Property(aa => aa.Building).HasColumnName("ShippingAddress_Building");

                // Configure the relationship with City
                a.HasOne(aa => aa.City)
                    .WithMany()
                    .HasForeignKey(aa => aa.CityId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Configure the relationship with State
                a.HasOne(aa => aa.State)
                    .WithMany()
                    .HasForeignKey(aa => aa.StateId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Configure the relationship with Country
                a.HasOne(aa => aa.Country)
                    .WithMany()
                    .HasForeignKey(aa => aa.CountryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}