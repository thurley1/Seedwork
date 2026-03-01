using Seedwork.Domain;

namespace Seedwork.Sample.Domain.Orders;

public class OrderStatus : Enumeration<OrderStatus>
{
    public static readonly OrderStatus Pending = new(0, "Pending");
    public static readonly OrderStatus Confirmed = new(1, "Confirmed");
    public static readonly OrderStatus Shipped = new(2, "Shipped");
    public static readonly OrderStatus Cancelled = new(3, "Cancelled");

    private OrderStatus(int value, string name) : base(value, name) { }
}
