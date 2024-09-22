namespace Ecommerce.Domain.Entities
{
    /// <summary>
    /// Represents a product category in the e-commerce system.
    /// </summary>
    public class Category : BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the category.
        /// </summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the category.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the collection of products in this category.
        /// </summary>
        public ICollection<Product> Products { get; set; }

        /// <summary>
        /// Initializes a new instance of the Category class.
        /// </summary>
        public Category()
        {
            CategoryId = Guid.NewGuid();
            Products = new List<Product>();
        }
    }
}