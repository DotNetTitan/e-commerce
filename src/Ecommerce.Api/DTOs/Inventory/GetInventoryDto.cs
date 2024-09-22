namespace Ecommerce.Api.DTOs.Inventory
{
    public class GetInventoryDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public bool? LowStockOnly { get; set; }
        public Guid? CategoryId { get; set; }
    }
}