namespace Ecommerce.Domain.Entities
{
    public class Product : BaseEntity
    {
        public Product()
        {
            ProductId = Guid.NewGuid();
            Reviews = new List<Review>();
        }

        public Guid ProductId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required decimal Price { get; set; }
        public required Guid CategoryId { get; set; }
        public Category? Category { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}