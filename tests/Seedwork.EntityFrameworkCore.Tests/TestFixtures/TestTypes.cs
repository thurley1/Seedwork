using Seedwork.Abstractions;
using Seedwork.Domain;

namespace Seedwork.EntityFrameworkCore.Tests.TestFixtures;

public record ProductId(Guid Value) : Id<ProductId>(Value);

public class ProductStatus : Enumeration<ProductStatus>
{
    public static readonly ProductStatus Draft = new(0, "Draft");
    public static readonly ProductStatus Active = new(1, "Active");
    public static readonly ProductStatus Discontinued = new(2, "Discontinued");

    private ProductStatus(int value, string name) : base(value, name) { }
}

public class Product : AggregateRoot<ProductId>
{
    public string Name { get; private set; } = string.Empty;
    public ProductStatus Status { get; private set; } = ProductStatus.Draft;

    public Product(ProductId id, string name) : base(id)
    {
        Name = name;
        RaiseDomainEvent(new ProductCreatedEvent(id));
    }

    private Product() { }

    public void Activate()
    {
        Status = ProductStatus.Active;
    }
}

public class ProductCreatedEvent : IDomainEvent
{
    public ProductId ProductId { get; }
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;

    public ProductCreatedEvent(ProductId productId)
    {
        ProductId = productId;
    }
}
