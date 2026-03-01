# Spec: DomainGuard

## Overview

Static class with 12 guard methods. All return the validated value for fluent chaining. All throw `DomainValidationException` with the parameter name.

## Methods

| Method | Validates | Throws when |
|--------|-----------|-------------|
| `AgainstNull<T>(T?, string)` | reference not null | `value is null` |
| `AgainstNullOrWhiteSpace(string?, string)` | string has content | null/empty/whitespace |
| `AgainstEmpty(Guid, string)` | Guid is non-empty | `Guid.Empty` |
| `AgainstLessThan(decimal, decimal, string)` | value >= minimum | `value < minimum` |
| `AgainstOutOfRange(int, int, int, string)` | min <= value <= max | outside range |
| `AgainstNegative(decimal, string)` | value >= 0 | negative |
| `AgainstNegativeOrZero(decimal, string)` | value > 0 | negative or zero |
| `AgainstInvalidFormat(string, string, string)` | matches regex | no match |
| `AgainstLength(string, int, string)` | length <= max | exceeds max |
| `AgainstLengthOutOfRange(string, int, int, string)` | min <= length <= max | outside range |
| `AgainstEmptyCollection<T>(IEnumerable<T>, string)` | has elements | empty |
| `AgainstInvalidEmail(string, string)` | valid email format | invalid |

## Behavior

- Every method returns the validated value as its first parameter type
- Every method throws `DomainValidationException` with structured `ParameterName`
- `AgainstNull` has `where T : class` constraint
- `AgainstInvalidEmail` uses a reasonable regex (not RFC 5322 full spec)
