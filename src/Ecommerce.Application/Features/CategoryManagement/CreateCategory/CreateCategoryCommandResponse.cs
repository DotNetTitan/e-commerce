namespace Ecommerce.Application.Features.CategoryManagement.CreateCategory
{
    public class CreateCategoryCommandResponse
    {
        public required Guid CategoryId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}