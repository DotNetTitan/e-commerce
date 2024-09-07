namespace Ecommerce.Application.Features.CategoryManagement.DeleteCategory
{
    public class DeleteCategoryResponse
    {
        public required Guid Id { get; set; }
        public required bool IsDeleted { get; set; }
    }
}