namespace Ecommerce.Domain.Entities
{
    public class ProductImage : BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the product image.
        /// </summary>
        public Guid ProductImageId { get; set; }

        /// <summary>
        /// Gets or sets the product identifier this image belongs to.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Gets or sets the URL of the image.
        /// </summary>
        public required string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the product this image belongs to.
        /// </summary>
        public Product? Product { get; set; }

        /// <summary>
        /// Initializes a new instance of the ProductImage class.
        /// </summary>
        public ProductImage()
        {
            ProductImageId = Guid.NewGuid();
        }
    }
}