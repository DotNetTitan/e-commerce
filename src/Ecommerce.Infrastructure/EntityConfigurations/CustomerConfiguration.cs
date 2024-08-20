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

            builder.OwnsOne(c => c.CustomerAddress, a =>
            {
                a.Property(aa => aa.Street).HasColumnName("CustomerAddress_Street");
                a.Property(aa => aa.PostalCode).HasColumnName("CustomerAddress_PostalCode");
                a.Property(aa => aa.Building).HasColumnName("CustomerAddress_Building");

                a.HasOne(aa => aa.City)
                    .WithMany()
                    .HasForeignKey(aa => aa.CityId)
                    .OnDelete(DeleteBehavior.Restrict);

                a.HasOne(aa => aa.State)
                    .WithMany()
                    .HasForeignKey(aa => aa.StateId)
                    .OnDelete(DeleteBehavior.Restrict);

                a.HasOne(aa => aa.Country)
                    .WithMany()
                    .HasForeignKey(aa => aa.CountryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}