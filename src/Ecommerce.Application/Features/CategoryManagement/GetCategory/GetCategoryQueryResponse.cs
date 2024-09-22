namespace Ecommerce.Application.Features.CategoryManagement.GetCategory
{
    public class GetCategoryQueryResponse
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}