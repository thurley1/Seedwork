# Spec: EF Core Integration

## Overview

Bridges Seedwork domain types with Entity Framework Core — value converters, comparers, configuration helpers, and domain event dispatch.

## Converters

### IdValueConverter\<TId\>
- Converts `Id<TId>` ↔ `Guid` for database storage
- Uses `Id<TId>.From()` for materialization

### IdValueConverterSelector
- Custom `ValueConverterSelector` that auto-discovers `Id<T>` subtypes
- Registers `IdValueConverter<TId>` for each discovered type
- Used in `ModelConfigurationBuilderExtensions`

### EnumerationValueConverter\<TEnum\>
- Converts `Enumeration<TEnum>` ↔ `int` using `Value` and `FromValue()`

## Comparers

### IdValueComparer\<TId\>
- `ValueComparer<TId>` for `Id<T>` types
- Compares by underlying `Guid` value

### ValueObjectComparer\<T\>
- `ValueComparer<T>` for `ValueObject` subtypes
- Uses `Equals` + `GetHashCode` + `GetCopy()` for snapshot

## Configuration Extensions

### EntityTypeConfigurationExtensions
- `ConfigureId<TEntity, TId>()` — sets up Id property with converter + comparer
- `ConfigureEnumeration<TEntity, TEnum>()` — sets up Enumeration property with converter

### ModelConfigurationBuilderExtensions
- `ConfigureIdConventions()` — registers IdValueConverterSelector for all Id types globally
- `ConfigureEnumerationConventions<TEnum>()` — registers converter for a specific enum type

### ModelBuilderExtensions
- `IgnoreDomainEvents()` — ignores DomainEvents navigation on all aggregate root types

## Interceptors

### DomainEventDispatchInterceptor
- `SaveChangesInterceptor` that collects domain events from changed entities
- Clears events, then dispatches them via `Func<IDomainEvent, CancellationToken, Task>`
- Dispatches after `SaveChangesAsync` succeeds (not before)
