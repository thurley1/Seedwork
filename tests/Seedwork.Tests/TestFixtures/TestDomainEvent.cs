using Seedwork.Abstractions;

namespace Seedwork.Tests.TestFixtures;

public class TestDomainEvent : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}
