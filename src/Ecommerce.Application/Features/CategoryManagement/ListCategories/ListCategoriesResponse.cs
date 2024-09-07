namespace Ecommerce.Application.Features.CategoryManagement.ListCategories
{
    public class ListCategoriesResponse
    {
        public required List<CategoryDetails> Categories { get; set; }
        public required int TotalCount { get; set; }
        public required int PageNumber { get; set; }
        public required int PageSize { get; set; }
    }

    public class CategoryDetails
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}