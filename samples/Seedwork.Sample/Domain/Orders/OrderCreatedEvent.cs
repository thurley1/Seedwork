using Seedwork.Abstractions;

namespace Seedwork.Sample.Domain.Orders;

public class OrderCreatedEvent : IDomainEvent
{
    public OrderId OrderId { get; }
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;

    public OrderCreatedEvent(OrderId orderId)
    {
        OrderId = orderId;
    }
}
