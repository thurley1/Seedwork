using Seedwork.Domain;

namespace Seedwork.Tests.TestFixtures;

public class TestEntity : Entity<Guid>
{
    public string Name { get; }

    public TestEntity(Guid id, string name) : base(id)
    {
        Name = name;
    }

    // Parameterless for ORM tests
    private TestEntity() { Name = string.Empty; }
}

public class OtherTestEntity : Entity<Guid>
{
    public OtherTestEntity(Guid id) : base(id) { }
}

public record TestId(Guid Value) : Id<TestId>(Value);

public class TestAggregate : AggregateRoot<Guid>
{
    public string Name { get; private set; }

    public TestAggregate(Guid id, string name) : base(id)
    {
        Name = name;
    }

    private TestAggregate() { Name = string.Empty; }

    public void DoSomething()
    {
        RaiseDomainEvent(new TestDomainEvent());
    }

    public void RaiseSpecificEvent(TestDomainEvent evt) => RaiseDomainEvent(evt);
}

public class TestAggregateWithTypedId : AggregateRoot<TestId>
{
    public TestAggregateWithTypedId(TestId id) : base(id) { }
    private TestAggregateWithTypedId() { }
}
