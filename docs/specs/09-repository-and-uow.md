# Spec: Repository & Unit of Work

## IRepository\<TAggregate, TId\>

- Constraint: `where TAggregate : class, IAggregateRoot`
- Property: `IUnitOfWork UnitOfWork { get; }`
- Methods:
  - `Task<TAggregate?> GetByIdAsync(TId id, CancellationToken)`
  - `Task AddAsync(TAggregate aggregate, CancellationToken)`
  - `Task UpdateAsync(TAggregate aggregate, CancellationToken)`
  - `Task DeleteAsync(TId id, CancellationToken)`

## IUnitOfWork

- Method: `Task<int> SaveChangesAsync(CancellationToken)`
