# Spec: Sample Project

## Overview

Demonstrates Seedwork usage with a simple Order aggregate:
- `OrderId` (strongly-typed Id)
- `OrderStatus` (Enumeration)
- `Money` (ValueObject)
- `Order` (AggregateRoot) with OrderItems
- EF Core configuration with InMemory provider
- Domain event dispatch

## Purpose

Serves as a runnable example and integration test.
