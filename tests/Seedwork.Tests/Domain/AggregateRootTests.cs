using FluentAssertions;
using Seedwork.Tests.TestFixtures;

namespace Seedwork.Tests.Domain;

public class AggregateRootTests
{
    [Fact]
    public void RaiseDomainEvent_AddsEventToCollection()
    {
        var aggregate = new TestAggregate(Guid.NewGuid(), "Test");

        aggregate.DoSomething();

        aggregate.DomainEvents.Should().HaveCount(1);
        aggregate.DomainEvents.Should().AllBeOfType<TestDomainEvent>();
    }

    [Fact]
    public void RaiseDomainEvent_Null_ThrowsArgumentNullException()
    {
        var aggregate = new TestAggregate(Guid.NewGuid(), "Test");

        var act = () => aggregate.RaiseSpecificEvent(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ClearDomainEvents_RemovesAllEvents()
    {
        var aggregate = new TestAggregate(Guid.NewGuid(), "Test");
        aggregate.DoSomething();
        aggregate.DoSomething();

        aggregate.ClearDomainEvents();

        aggregate.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void RemoveDomainEvent_RemovesSpecificEvent()
    {
        var aggregate = new TestAggregate(Guid.NewGuid(), "Test");
        var evt = new TestDomainEvent();
        aggregate.RaiseSpecificEvent(evt);
        aggregate.DoSomething();

        aggregate.RemoveDomainEvent(evt);

        aggregate.DomainEvents.Should().HaveCount(1);
        aggregate.DomainEvents.Should().NotContain(evt);
    }

    [Fact]
    public void DomainEvents_IsNeverNull()
    {
        var aggregate = new TestAggregate(Guid.NewGuid(), "Test");

        aggregate.DomainEvents.Should().NotBeNull();
        aggregate.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void InheritsEntityEquality()
    {
        var id = Guid.NewGuid();
        var a = new TestAggregate(id, "A");
        var b = new TestAggregate(id, "B");

        a.Should().Be(b);
    }
}
