namespace Ecommerce.Application.Features.CategoryManagement.GetCategoryDetails
{
    public class GetCategoryDetailsResponse
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}