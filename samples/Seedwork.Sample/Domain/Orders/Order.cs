using Seedwork.Domain;
using Seedwork.Guard;

namespace Seedwork.Sample.Domain.Orders;

public class Order : AggregateRoot<OrderId>
{
    private readonly List<OrderItem> _items = [];

    public string CustomerName { get; private set; }
    public OrderStatus Status { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    public static Order Create(string customerName)
    {
        DomainGuard.AgainstNullOrWhiteSpace(customerName, nameof(customerName));

        var order = new Order(OrderId.New(), customerName);
        order.RaiseDomainEvent(new OrderCreatedEvent(order.Id));
        return order;
    }

    private Order(OrderId id, string customerName) : base(id)
    {
        CustomerName = customerName;
        Status = OrderStatus.Pending;
    }

    private Order()
    {
        CustomerName = string.Empty;
        Status = OrderStatus.Pending;
    }

    public void AddItem(string productName, int quantity, Money unitPrice)
    {
        var item = new OrderItem(Guid.NewGuid(), productName, quantity, unitPrice);
        _items.Add(item);
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Only pending orders can be confirmed.");

        if (_items.Count == 0)
            throw new InvalidOperationException("Cannot confirm an order with no items.");

        Status = OrderStatus.Confirmed;
    }
}
