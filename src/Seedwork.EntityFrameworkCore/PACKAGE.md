# Seedwork.EntityFrameworkCore

EF Core integration for [Seedwork](https://www.nuget.org/packages/Seedwork) DDD building blocks.

## What's Included

| Namespace | Types |
|-----------|-------|
| `Seedwork.EntityFrameworkCore.Converters` | `IdValueConverter<TId>`, `EnumerationValueConverter<TEnum>`, `IdValueConverterSelector` |
| `Seedwork.EntityFrameworkCore.Comparers` | `IdValueComparer<TId>`, `ValueObjectComparer<T>` |
| `Seedwork.EntityFrameworkCore.Configuration` | `ConfigureId()`, `ConfigureEnumeration()`, `ConfigureIdConventions()`, `ConfigureEnumerationConventions<T>()`, `IgnoreDomainEvents()` |
| `Seedwork.EntityFrameworkCore.Interceptors` | `DomainEventDispatchInterceptor` |

## Setup

### Convention-Based (Recommended)

Register converters globally — all `Id<T>` and `Enumeration<TEnum>` properties are auto-configured:

```csharp
public class AppDbContext : DbContext
{
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.ConfigureIdConventions();                    // all Id<T> → Guid
        configurationBuilder.ConfigureEnumerationConventions<OrderStatus>(); // OrderStatus → int
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.IgnoreDomainEvents(); // ignore DomainEvents navigation
    }
}
```

### Per-Property (Fine-Grained Control)

Configure individual properties in `IEntityTypeConfiguration<T>`:

```csharp
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).ConfigureId();
        builder.Property(o => o.Status).ConfigureEnumeration();
    }
}
```

### Domain Event Dispatch

Dispatch domain events after `SaveChangesAsync` succeeds:

```csharp
var interceptor = new DomainEventDispatchInterceptor(async (evt, ct) =>
{
    await mediator.Publish(evt, ct);  // MediatR, event bus, etc.
});

services.AddDbContext<AppDbContext>(opts =>
    opts.AddInterceptors(interceptor));
```

The interceptor:
1. Collects domain events from all changed `IAggregateRoot` entities
2. Clears events from the aggregates
3. Dispatches each event via your callback after save succeeds

### Value Object Change Tracking

Use `ValueObjectComparer<T>` for owned value objects to enable proper EF Core change tracking:

```csharp
builder.OwnsOne(o => o.ShippingAddress, sa =>
{
    sa.Property(a => a.Street);
    sa.Property(a => a.City);
});
```

`ValueObject.GetCopy()` provides snapshot support for the comparer.

## Requirements

- .NET 10+
- EF Core 10 (preview)
- [Seedwork](https://www.nuget.org/packages/Seedwork) 1.0.0+

## License

MIT
