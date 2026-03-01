using Seedwork.Domain;
using Seedwork.Guard;

namespace Seedwork.Sample.Domain.Orders;

public class OrderItem : Entity<Guid>
{
    public string ProductName { get; private set; }
    public int Quantity { get; private set; }
    public Money UnitPrice { get; private set; }

    public Money LineTotal => new(UnitPrice.Amount * Quantity, UnitPrice.Currency);

    internal OrderItem(Guid id, string productName, int quantity, Money unitPrice) : base(id)
    {
        DomainGuard.AgainstNullOrWhiteSpace(productName, nameof(productName));
        DomainGuard.AgainstNegativeOrZero(quantity, nameof(quantity));

        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    private OrderItem()
    {
        ProductName = string.Empty;
        UnitPrice = null!;
    }
}
