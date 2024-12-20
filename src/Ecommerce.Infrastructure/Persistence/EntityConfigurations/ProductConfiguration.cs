﻿using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Persistence.EntityConfigurations
{
    internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.ProductId)
                .IsClustered(false);

            builder.Property<int>("ClusterId")
                .UseIdentityColumn();

            builder.HasIndex("ClusterId")
                .IsClustered();

            builder.Property(p => p.Name)
                .IsRequired();

            builder.Property(p => p.Price)
                .HasColumnType("decimal(18, 2)");

            builder.HasMany(p => p.Reviews)
                .WithOne(r => r.Product)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => p.Name);
            builder.HasIndex(p => p.CategoryId);

            builder.HasMany(p => p.Images)
                .WithOne(i => i.Product)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}