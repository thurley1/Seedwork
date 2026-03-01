# Seedwork

DDD building blocks for .NET — Entity, ValueObject, AggregateRoot, Id, Guard clauses, and more.

## Packages

| Package | Description |
|---------|-------------|
| `Seedwork` | Core library (zero dependencies) — entities, value objects, aggregate roots, guard clauses, exceptions |
| `Seedwork.EntityFrameworkCore` | EF Core integration — value converters, comparers, configuration extensions, domain event dispatch |

## Quick Start

### Define a strongly-typed Id

```csharp
public record OrderId(Guid Value) : Id<OrderId>(Value);
```

### Define an Entity

```csharp
public class OrderItem : Entity<Guid>
{
    public string ProductName { get; }
    public int Quantity { get; }

    public OrderItem(Guid id, string productName, int quantity) : base(id)
    {
        ProductName = DomainGuard.AgainstNullOrWhiteSpace(productName, nameof(productName));
        Quantity = (int)DomainGuard.AgainstNegativeOrZero(quantity, nameof(quantity));
    }
}
```

### Define a ValueObject

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

### Define an AggregateRoot

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

### Define a Smart Enum

```csharp
public class OrderStatus : Enumeration<OrderStatus>
{
    public static readonly OrderStatus Pending = new(0, "Pending");
    public static readonly OrderStatus Confirmed = new(1, "Confirmed");
    public static readonly OrderStatus Shipped = new(2, "Shipped");

    private OrderStatus(int value, string name) : base(value, name) { }
}

// Usage
var status = OrderStatus.FromName("Confirmed");
var all = OrderStatus.GetAll();
```

### EF Core Configuration

```csharp
protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
{
    configurationBuilder.ConfigureIdConventions();
    configurationBuilder.ConfigureEnumerationConventions<OrderStatus>();
}

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.IgnoreDomainEvents();
}
```

### Domain Event Dispatch

```csharp
var interceptor = new DomainEventDispatchInterceptor(async (evt, ct) =>
{
    // dispatch to MediatR, event bus, etc.
    await mediator.Publish(evt, ct);
});

services.AddDbContext<AppDbContext>(opts =>
    opts.AddInterceptors(interceptor));
```

## Guard Clauses

`DomainGuard` provides 12 fluent validation methods that all return the validated value:

```csharp
Name = DomainGuard.AgainstNullOrWhiteSpace(name, nameof(name));
Email = DomainGuard.AgainstInvalidEmail(email, nameof(email));
Quantity = (int)DomainGuard.AgainstNegativeOrZero(quantity, nameof(quantity));
Title = DomainGuard.AgainstLength(title, 200, nameof(title));
```

All throw `DomainValidationException` with a `ParameterName` property for structured API error responses.

## Exception Hierarchy

- `DomainException` — base domain error
  - `DomainValidationException` — validation failure with `ParameterName`
  - `EntityNotFoundException` — entity not found with `EntityType` and `EntityId`
  - `ForbiddenException` — authorization failure

```csharp
throw EntityNotFoundException.For<Order>(orderId);
```

## Building

```bash
dotnet build
dotnet test
dotnet pack -c Release -o ./nupkg
```

## License

[MIT](LICENSE)
