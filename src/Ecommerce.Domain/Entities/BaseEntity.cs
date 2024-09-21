namespace Ecommerce.Domain.Entities
{
    public abstract class BaseEntity
    {
        public DateTimeOffset CreatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public DateTimeOffset? LastModifiedAt { get; set; }

        public string? LastModifiedBy { get; set; }
    }
}