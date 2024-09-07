namespace Ecommerce.Application.Features.CategoryManagement.CreateCategory
{
    public class CreateCategoryResponse
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}