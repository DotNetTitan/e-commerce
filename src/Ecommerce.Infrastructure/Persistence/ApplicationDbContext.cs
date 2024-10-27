using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Persistence.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Ecommerce.Application.Common.Models;

namespace Ecommerce.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole<string>, string>
    {
        private readonly ICurrentUserService? _currentUserService;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
            base(options)
        { }

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityUser>()
                .Property(u => u.Id)
                .HasColumnName("IdentityId");

            // Apply entity configurations
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new CustomerConfiguration());
            builder.ApplyConfiguration(new OrderConfiguration());
            builder.ApplyConfiguration(new OrderItemConfiguration());
            builder.ApplyConfiguration(new ProductConfiguration());
            builder.ApplyConfiguration(new ReviewConfiguration());
            builder.ApplyConfiguration(new ShoppingCartConfiguration());
            builder.ApplyConfiguration(new ShoppingCartItemConfiguration());
            builder.ApplyConfiguration(new ProductImageConfiguration());
        }

        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditFields()
        {
            var currentTime = DateTimeOffset.UtcNow;
            var currentUser = GetCurrentUser();

            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = currentTime;
                        entry.Entity.CreatedBy = currentUser;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedAt = currentTime;
                        entry.Entity.LastModifiedBy = currentUser;
                        break;
                }
            }
        }

        private string GetCurrentUser()
        {
            return _currentUserService?.UserName ?? "system";
        }
    }
}
