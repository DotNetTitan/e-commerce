﻿using Ecommerce.Domain.Enums;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Domain.Entities
{
    /// <summary>
    /// Represents an order in the e-commerce system.
    /// </summary>
    public class Order : BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the order.
        /// </summary>
        public Guid OrderId { get; }

        /// <summary>
        /// Gets or sets the date and time when the order was placed.
        /// </summary>
        public DateTime OrderDate { get; }

        /// <summary>
        /// Gets or sets the unique order number.s
        /// </summary>
        public string OrderNumber { get; }

        /// <summary>
        /// Gets or sets the unique tracking number.
        /// </summary>
        public string TrackingNumber { get; }

        /// <summary>
        /// Gets or sets the customer identifier associated with the order.
        /// </summary>
        public required Guid CustomerId { get; init; }

        /// <summary>
        /// Gets or sets the status of the order.
        /// </summary>
        public OrderStatus OrderStatus { get; private set; }

        /// <summary>
        /// Gets or sets the status of the payment.
        /// </summary>
        public PaymentStatus PaymentStatus { get; private set; }

        /// <summary>
        /// Gets or sets the customer associated with the order.
        /// </summary>
        public Customer? Customer { get; init; }

        /// <summary>
        /// Gets or sets  the collection of order items.
        /// </summary>
        public ICollection<OrderItem> OrderItems { get; init; }

        /// <summary>
        /// Gets or sets the shipping address for the order.
        /// </summary>
        public required Address ShippingAddress { get; init; }

        /// <summary>
        /// Initializes a new instance of the Order class.
        /// </summary>
        public Order()
        {
            OrderId = Guid.NewGuid();
            OrderItems = new List<OrderItem>();
            OrderStatus = OrderStatus.Pending;
            PaymentStatus = PaymentStatus.Pending;
            PaymentMethod = PaymentMethod.CashOnDelivery;
            OrderDate = DateTime.UtcNow;
            OrderNumber = GenerateOrderNumber();
            TrackingNumber = GenerateTrackingNumber();
        }

        /// <summary>
        /// Generates a unique order number using the OrderId and a precise timestamp.
        /// </summary>
        /// <returns>A string representing the unique order number.</returns>
        private string GenerateOrderNumber()
        {
            var datePrefix = OrderDate.ToString("yyyyMMddHHmmssfff");
            var guidSuffix = OrderId.ToString("N").Substring(0, 8).ToUpper();
            return $"ORD-{datePrefix}-{guidSuffix}";
        }

        /// <summary>
        /// Generates a unique tracking number.
        /// </summary>
        /// <returns>A string representing the unique tracking number.</returns>
        public string GenerateTrackingNumber()
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var uniqueId = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            return
                $"TRK-{OrderId.ToString().Substring(0, 8).ToUpper()}-{CustomerId.ToString().Substring(0, 8).ToUpper()}-{timestamp}-{uniqueId}";
        }

        /// <summary>
        /// Gets or sets the total amount of the order.
        /// </summary>
        public decimal TotalAmount { get; private set; }

        /// <summary>
        /// Calculates and updates the total amount of the order.
        /// </summary>
        private void CalculateTotalAmount()
        {
            TotalAmount = OrderItems.Sum(item => (item.Quantity* item.UnitPrice));
        }

        /// <summary>
        /// Adds an order item to the order.
        /// </summary>
        /// <param name="item">The order item to add.</param>
        public void AddOrderItem(OrderItem item)
        {
            OrderItems.Add(item);
            CalculateTotalAmount();
        }

        /// <summary>
        /// Removes an order item from the order.
        /// </summary>
        /// <param name="item">The order item to remove.</param>
        public void RemoveOrderItem(OrderItem item)
        {
            OrderItems.Remove(item);
            CalculateTotalAmount();
        }

        /// <summary>
        /// Updates the status of the order.
        /// </summary>
        /// <param name="newStatus">The new status of the order.</param>
        public void UpdateOrderStatus(OrderStatus newStatus)
        {
            if (newStatus == OrderStatus)
                return;

            switch (OrderStatus)
            {
                case OrderStatus.Pending:
                    if (newStatus == OrderStatus.Processing || newStatus == OrderStatus.Cancelled)
                        OrderStatus = newStatus;
                    else
                        throw new InvalidOperationException($"Invalid status transition from {OrderStatus} to {newStatus}");
                    break;
                case OrderStatus.Processing:
                    if (newStatus == OrderStatus.Shipped || newStatus == OrderStatus.Cancelled)
                        OrderStatus = newStatus;
                    else
                        throw new InvalidOperationException($"Invalid status transition from {OrderStatus} to {newStatus}");
                    break;
                case OrderStatus.Shipped:
                    if (newStatus == OrderStatus.Delivered)
                        OrderStatus = newStatus;
                    else
                        throw new InvalidOperationException($"Invalid status transition from {OrderStatus} to {newStatus}");
                    break;
                default:
                    throw new InvalidOperationException($"Invalid status transition from {OrderStatus} to {newStatus}");
            }
        }

        /// <summary>
        /// Updates the status of the payment.
        /// </summary>
        /// <param name="newStatus">The new status of the payment.</param>
        public void UpdatePaymentStatus(PaymentStatus newStatus)
        {
            if (newStatus == PaymentStatus)
                return;

            switch (PaymentStatus)
            {
                case PaymentStatus.Pending:
                    if (newStatus == PaymentStatus.Paid)
                        PaymentStatus = newStatus;
                    else
                        throw new InvalidOperationException($"Invalid payment status transition from {PaymentStatus} to {newStatus}");
                    break;
                case PaymentStatus.Paid:
                    if (newStatus == PaymentStatus.Refunded)
                        PaymentStatus = newStatus;
                    else
                        throw new InvalidOperationException($"Invalid payment status transition from {PaymentStatus} to {newStatus}");
                    break;
                default:
                    throw new InvalidOperationException($"Invalid payment status transition from {PaymentStatus} to {newStatus}");
            }
        }

        public void CancelOrder()
        {
            switch (OrderStatus)
            {
                case OrderStatus.Pending:
                case OrderStatus.Processing:
                    OrderStatus = OrderStatus.Cancelled;
                    PaymentStatus = PaymentStatus.Pending;
                    break;
                case OrderStatus.Shipped:
                    throw new InvalidOperationException("Cannot cancel an order that has been shipped.");
                case OrderStatus.Delivered:
                    throw new InvalidOperationException("Cannot cancel an order that has been delivered.");
                case OrderStatus.Cancelled:
                    throw new InvalidOperationException("Order is already cancelled.");
                default:
                    throw new InvalidOperationException($"Cannot cancel order with status {OrderStatus}.");
            }
        }

        /// <summary>
        /// Checks if the order is empty.
        /// </summary>
        /// <returns>True if the order has no items; otherwise, false.</returns>
        public bool IsEmpty() => OrderItems.Count == 0;

        /// <summary>
        /// Gets the total number of items in the order.
        /// </summary>
        public int ItemCount => OrderItems.Sum(item => item.Quantity);

        /// <summary>
        /// Gets the number of unique products in the order.
        /// </summary>
        public int UniqueItemCount => OrderItems.Count;

        /// <summary>
        /// Validates the total amount of the order against an expected total.
        /// </summary>
        /// <param name="expectedTotal">The expected total amount.</param>
        /// <returns>True if the total amount matches the expected total; otherwise, false.</returns>
        public bool ValidateTotalAmount(decimal expectedTotal)
        {
            return TotalAmount == expectedTotal;
        }

        public PaymentMethod PaymentMethod { get; init; }
    }
}