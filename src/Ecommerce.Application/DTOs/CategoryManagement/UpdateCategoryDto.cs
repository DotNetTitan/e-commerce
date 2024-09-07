namespace Ecommerce.Application.DTOs.CategoryManagement
{
    public class UpdateCategoryDto
    {
        public required Guid CategoryId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}