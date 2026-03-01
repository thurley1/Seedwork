# Spec: Enumeration\<TEnum\>

## Overview

Smart enum pattern — finite set of named instances with an `int` value.

## API

- `int Value { get; }` — numeric identifier
- `string Name { get; }` — display name
- Constructor: `protected Enumeration(int value, string name)`
- `static IReadOnlyCollection<TEnum> GetAll()` — returns all declared instances
- `static TEnum FromValue(int value)` — finds by value or throws
- `static TEnum FromName(string name)` — finds by name (case-insensitive) or throws
- `static bool TryFromValue(int value, out TEnum? result)`
- `static bool TryFromName(string name, out TEnum? result)`
- Equality: based on `Value`
- `IComparable<Enumeration<TEnum>>` — compares by `Value`
- `ToString()` — returns `Name`

## Constraints

- `where TEnum : Enumeration<TEnum>`

## Behavior

- `GetAll()` uses cached reflection (public static fields of type TEnum)
- `FromValue` / `FromName` throw `InvalidOperationException` if not found
- `TryFromValue` / `TryFromName` return false if not found
- Two instances with same Value are equal regardless of Name
