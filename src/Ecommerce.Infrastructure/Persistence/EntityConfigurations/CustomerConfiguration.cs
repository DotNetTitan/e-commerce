using Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Persistence.EntityConfigurations
{
    internal sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.CustomerId)
                .IsClustered(false);

            builder.Property<int>("ClusterId")
                .UseIdentityColumn();

            builder.HasIndex("ClusterId")
                .IsClustered();

            builder.Property(c => c.IdentityId)
                .IsRequired()
                .HasColumnName("IdentityId");

            builder.HasOne<IdentityUser>()
                .WithOne()
                .HasForeignKey<Customer>(c => c.IdentityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.ShoppingCart)
                .WithOne(sc => sc.Customer)
                .HasForeignKey<ShoppingCart>(sc => sc.CustomerId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasMany(c => c.Reviews)
                .WithOne(r => r.Customer)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.OwnsOne(c => c.CustomerAddress, address =>
            {
                address.Property(a => a.Building).HasColumnName("CustomerAddressBuilding");
                address.Property(a => a.Street).HasColumnName("CustomerAddressStreet");
                address.Property(a => a.PostalCode).HasColumnName("CustomerAddressPostalCode");
                address.Property(a => a.City).HasColumnName("CustomerAddressCity");
                address.Property(a => a.State).HasColumnName("CustomerAddressState");
                address.Property(a => a.Country).HasColumnName("CustomerAddressCountry");
            });

            builder.HasIndex(c => c.IdentityId);
        }
    }
}