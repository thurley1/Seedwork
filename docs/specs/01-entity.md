# Spec: Entity\<TId\>

## Overview

Abstract base class for domain entities. Identity-based equality.

## API

- `TId Id { get; protected init; }` — the entity's identity
- Constructor: `protected Entity(TId id)` — sets Id
- Constructor: `protected Entity()` — parameterless for ORM
- Implements `IEquatable<Entity<TId>>`
- `Equals(Entity<TId>?)` — true if same type and same Id
- `Equals(object?)` — delegates to typed Equals with type check
- `GetHashCode()` — based on Id
- `operator ==` and `operator !=`
- `ToString()` — `"TypeName [Id]"`

## Constraints

- `where TId : notnull`

## Behavior

- Two entities of the same concrete type with the same Id are equal
- Two entities of different concrete types with the same Id are NOT equal
- Null comparisons return false
- Reference equality short-circuits
- Parameterless constructor leaves Id as `default` (for ORM hydration)
