namespace Ecommerce.Application.Features.CategoryManagement.UpdateCategory
{
    public class UpdateCategoryCommandResponse
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}