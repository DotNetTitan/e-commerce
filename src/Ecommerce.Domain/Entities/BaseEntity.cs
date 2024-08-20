namespace Ecommerce.Domain.Entities
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            Created = DateTimeOffset.UtcNow;
        }

        public DateTimeOffset Created { get; set; }

        public required string CreatedBy { get; set; }

        public DateTimeOffset? LastModified { get; set; }

        public string? LastModifiedBy { get; set; }
    }
}