namespace Ecommerce.Application.Features.CategoryManagement.DeleteCategory
{
    public class DeleteCategoryCommandResponse
    {
        public required Guid CategoryId { get; set; }
        public required bool IsDeleted { get; set; }
    }
}