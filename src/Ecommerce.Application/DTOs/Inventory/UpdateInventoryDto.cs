namespace Ecommerce.Application.DTOs.Inventory
{
    public class UpdateInventoryDto
    {
        public required Guid ProductId { get; init; }
        public required int NewQuantity { get; init; }
        public required int LowStockThreshold { get; init; }
    }
}