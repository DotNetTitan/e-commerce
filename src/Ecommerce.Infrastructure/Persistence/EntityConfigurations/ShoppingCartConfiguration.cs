using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Persistence.EntityConfigurations
{
    internal sealed class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
    {
        public void Configure(EntityTypeBuilder<ShoppingCart> builder)
        {
            builder.HasKey(sc => sc.ShoppingCartId);

            builder.HasMany(sc => sc.ShoppingCartItems)
                .WithOne(sci => sci.ShoppingCart)
                .HasForeignKey(sci => sci.ShoppingCartId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(sc => sc.Customer)
                .WithOne(c => c.ShoppingCart)
                .HasForeignKey<ShoppingCart>(sc => sc.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(sc => sc.CustomerId);
        }
    }
}