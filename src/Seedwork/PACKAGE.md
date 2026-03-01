# Seedwork

DDD building blocks for .NET — zero external dependencies.

## What's Included

| Namespace | Types |
|-----------|-------|
| `Seedwork.Domain` | `Entity<TId>`, `ValueObject`, `AggregateRoot<TId>`, `Id<T>`, `Enumeration<TEnum>` |
| `Seedwork.Abstractions` | `IDomainEvent`, `IAggregateRoot`, `IRepository<,>`, `IUnitOfWork` |
| `Seedwork.Guard` | `DomainGuard` (12 fluent validation methods) |
| `Seedwork.Exceptions` | `DomainException`, `DomainValidationException`, `EntityNotFoundException`, `ForbiddenException` |

## Quick Start

### Strongly-typed Id

```csharp
using Seedwork.Domain;

public record OrderId(Guid Value) : Id<OrderId>(Value);

var id = OrderId.New();           // random
var id2 = OrderId.From(someGuid); // from existing
var id3 = OrderId.Parse("...");   // from string
Guid raw = id;                    // implicit conversion
```

### Entity

```csharp
public class OrderItem : Entity<Guid>
{
    public string ProductName { get; private set; }

    public OrderItem(Guid id, string productName) : base(id)
    {
        ProductName = DomainGuard.AgainstNullOrWhiteSpace(productName, nameof(productName));
    }

    private OrderItem() { ProductName = string.Empty; } // ORM
}
```

### ValueObject

```csharp
public class Money : ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}
```

### AggregateRoot

```csharp
public class Order : AggregateRoot<OrderId>
{
    public string CustomerName { get; private set; }

    public static Order Create(string customerName)
    {
        var order = new Order(OrderId.New(), customerName);
        order.RaiseDomainEvent(new OrderCreatedEvent(order.Id));
        return order;
    }

    private Order(OrderId id, string customerName) : base(id)
    {
        CustomerName = DomainGuard.AgainstNullOrWhiteSpace(customerName, nameof(customerName));
    }

    private Order() { CustomerName = string.Empty; } // ORM
}
```

### Smart Enum

```csharp
public class OrderStatus : Enumeration<OrderStatus>
{
    public static readonly OrderStatus Pending = new(0, "Pending");
    public static readonly OrderStatus Confirmed = new(1, "Confirmed");
    public static readonly OrderStatus Shipped = new(2, "Shipped");

    private OrderStatus(int value, string name) : base(value, name) { }
}

var status = OrderStatus.FromName("Confirmed");
var all = OrderStatus.GetAll();
bool found = OrderStatus.TryFromValue(99, out var result); // false
```

### Guard Clauses

All 12 methods return the validated value and throw `DomainValidationException` on failure:

```csharp
Name  = DomainGuard.AgainstNullOrWhiteSpace(name, nameof(name));
Email = DomainGuard.AgainstInvalidEmail(email, nameof(email));
Price = DomainGuard.AgainstNegativeOrZero(price, nameof(price));
Title = DomainGuard.AgainstLength(title, 200, nameof(title));
Code  = DomainGuard.AgainstInvalidFormat(code, @"^[A-Z]{3}$", nameof(code));
Qty   = DomainGuard.AgainstOutOfRange(qty, 1, 100, nameof(qty));
Items = DomainGuard.AgainstEmptyCollection(items, nameof(items));
```

### Exceptions

```csharp
// Validation — carries ParameterName for structured API errors
throw new DomainValidationException("Too short", "username");

// Entity not found — carries EntityType + EntityId
throw EntityNotFoundException.For<Order>(orderId);

// Authorization
throw new ForbiddenException("You do not own this resource.");
```

## EF Core Integration

For EF Core support (value converters, comparers, domain event dispatch), install `Seedwork.EntityFrameworkCore`.

## License

MIT
