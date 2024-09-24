using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Persistence.EntityConfigurations
{
    internal sealed class ShoppingCartItemConfiguration : IEntityTypeConfiguration<ShoppingCartItem>
    {
        public void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
        {
            builder.HasKey(sci => sci.ShoppingCartItemId);

            builder.Property<int>("ClusterId")
                .UseIdentityColumn();

            builder.HasIndex("ClusterId").IsClustered();

            builder.Property(sci => sci.Quantity).IsRequired();

            // Add this line to specify the column type for Price
            builder.Property(sci => sci.Price)
                .HasColumnType("decimal(18, 2)");

            builder.HasOne(sci => sci.ShoppingCart)
                .WithMany(sc => sc.ShoppingCartItems)
                .HasForeignKey(sci => sci.ShoppingCartId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(sci => sci.Product)
                .WithMany()
                .HasForeignKey(sci => sci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(sci => new { sci.ShoppingCartId, sci.ProductId });
        }
    }
}