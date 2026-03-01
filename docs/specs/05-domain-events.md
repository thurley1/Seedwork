# Spec: Domain Events & Aggregate Root Interface

## IDomainEvent

- Property: `DateTime OccurredOnUtc { get; }` — UTC timestamp
- Marker interface for domain events

## IAggregateRoot

- Property: `IReadOnlyCollection<IDomainEvent> DomainEvents { get; }`
- Method: `void ClearDomainEvents()`
- Used as a constraint on `IRepository` to decouple from concrete `AggregateRoot<TId>`
