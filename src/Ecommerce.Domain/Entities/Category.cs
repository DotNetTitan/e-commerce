namespace Ecommerce.Domain.Entities
{
    public class Category : BaseEntity
    {
        public Category()
        {
            CategoryId = Guid.NewGuid();
            Products = new List<Product>();
        }

        public Guid CategoryId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}