namespace Ecommerce.Domain.Events;

public class OrderCancelledEvent
{
    public Guid OrderId { get; init; }
    public Guid CustomerId { get; init; }
    public required string Email { get; init; }
}