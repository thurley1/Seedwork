# Spec: ValueObject

## Overview

Abstract base class for value objects. Structural equality based on components.

## API

- `abstract IEnumerable<object?> GetEqualityComponents()` — subclass defines components
- Implements `IEquatable<ValueObject>`
- `Equals(ValueObject?)` — true if same type and same components
- `Equals(object?)` — delegates
- `GetHashCode()` — combines all component hashes using `HashCode`
- `operator ==` and `operator !=`
- `GetCopy()` — returns a memberwise clone for EF Core change tracking

## Behavior

- Two value objects of the same type with the same components are equal
- Different types are never equal, even with same components
- Null is never equal
- Components can include null values
- `GetCopy()` produces a separate instance that compares equal
