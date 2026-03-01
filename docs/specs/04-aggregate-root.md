# Spec: AggregateRoot\<TId\>

## Overview

Extends `Entity<TId>`, implements `IAggregateRoot`. Manages domain events.

## API

- Inherits all Entity behavior
- Implements `IAggregateRoot`
- `IReadOnlyCollection<IDomainEvent> DomainEvents { get; }`
- `protected void RaiseDomainEvent(IDomainEvent domainEvent)` — adds event, guards null
- `void ClearDomainEvents()` — clears all events
- `void RemoveDomainEvent(IDomainEvent domainEvent)` — removes specific event
- Constructor: `protected AggregateRoot(TId id)` — passes to Entity
- Constructor: `protected AggregateRoot()` — parameterless for ORM

## Behavior

- Events accumulate via `RaiseDomainEvent`
- `ClearDomainEvents` empties the list
- `RemoveDomainEvent` removes a single event
- `RaiseDomainEvent(null)` throws `ArgumentNullException`
- DomainEvents collection is never null
