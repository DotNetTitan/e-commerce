namespace Ecommerce.Application.DTOs.CategoryManagement
{
    public class CreateCategoryDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}