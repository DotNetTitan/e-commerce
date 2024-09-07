namespace Ecommerce.Domain.Entities
{
    public class Review : BaseEntity
    {
        public Review()
        {
            ReviewId = Guid.NewGuid();
        }

        public Guid ReviewId { get; set; }
        public required Guid ProductId { get; set; }
        public Product? Product { get; set; }
        public required Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public required string Comment { get; set; }
        public required int Rating { get; set; }
        public required DateTime ReviewDate { get; set; }
    }
}