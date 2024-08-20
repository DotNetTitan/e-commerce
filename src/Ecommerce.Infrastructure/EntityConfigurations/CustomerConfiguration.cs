using Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.EntityConfigurations
{
    internal sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.CustomerId);
            builder.Property(c => c.IdentityId).IsRequired();
            builder.HasIndex(c => c.IdentityId).IsUnique();

            // Configure the IdentityId property
            builder.Property(c => c.IdentityId)
                .IsRequired()
                .HasColumnName("IdentityId");

            // Configure the one-to-one relationship with IdentityUser
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
        }
    }
}