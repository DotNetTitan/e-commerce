namespace Ecommerce.Application.Features.ProductManagement.DeleteProduct
{
    public class DeleteProductResponse
    {
        public required Guid ProductId { get; set; }
        public required bool IsDeleted { get; set; }
    }
}