namespace Ecommerce.Application.DTOs.CategoryManagement
{
    public class UpdateCategoryDto
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}