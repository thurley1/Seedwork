# Spec: Exception Classes

## Overview

Four domain-specific exception types that carry structured metadata for API error responses.

## Classes

### DomainException

- Inherits `Exception`
- Constructor: `(string message)`
- Constructor: `(string message, Exception innerException)`
- Base for all domain exceptions

### DomainValidationException

- Inherits `DomainException`
- Property: `string ParameterName { get; }` — the name of the invalid parameter
- Constructor: `(string message, string parameterName)`
- Used by `DomainGuard` methods

### EntityNotFoundException

- Inherits `DomainException`
- Property: `Type EntityType { get; }` — the type of entity not found
- Property: `object? EntityId { get; }` — the ID that was searched
- Constructor: `(Type entityType, object? entityId)`
  - Message: `"{entityType.Name} with id '{entityId}' was not found."`
- Static factory: `For<TEntity>(object? id)` — shorthand

### ForbiddenException

- Inherits `DomainException`
- Constructor: `(string message)`
- Represents authorization failures

## Behavior

- All exceptions are serializable by default (inherit from Exception)
- All preserve inner exceptions when provided
- `DomainValidationException.ParameterName` is never null/empty (guard in ctor)
- `EntityNotFoundException` message is deterministic and structured
