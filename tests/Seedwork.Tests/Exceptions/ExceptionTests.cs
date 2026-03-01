using FluentAssertions;
using Seedwork.Exceptions;

namespace Seedwork.Tests.Exceptions;

public class DomainExceptionTests
{
    [Fact]
    public void Constructor_SetsMessage()
    {
        var ex = new DomainException("something failed");

        ex.Message.Should().Be("something failed");
    }

    [Fact]
    public void Constructor_SetsMessageAndInnerException()
    {
        var inner = new InvalidOperationException("inner");
        var ex = new DomainException("outer", inner);

        ex.Message.Should().Be("outer");
        ex.InnerException.Should().BeSameAs(inner);
    }
}

public class DomainValidationExceptionTests
{
    [Fact]
    public void Constructor_SetsMessageAndParameterName()
    {
        var ex = new DomainValidationException("bad value", "email");

        ex.Message.Should().Be("bad value");
        ex.ParameterName.Should().Be("email");
    }

    [Fact]
    public void Constructor_InheritsFromDomainException()
    {
        var ex = new DomainValidationException("bad", "name");

        ex.Should().BeAssignableTo<DomainException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ThrowsWhenParameterNameIsNullOrWhiteSpace(string? parameterName)
    {
        var act = () => new DomainValidationException("msg", parameterName!);

        act.Should().Throw<ArgumentException>();
    }
}

public class EntityNotFoundExceptionTests
{
    [Fact]
    public void Constructor_SetsPropertiesAndMessage()
    {
        var id = Guid.NewGuid();
        var ex = new EntityNotFoundException(typeof(string), id);

        ex.EntityType.Should().Be(typeof(string));
        ex.EntityId.Should().Be(id);
        ex.Message.Should().Be($"String with id '{id}' was not found.");
    }

    [Fact]
    public void Constructor_HandlesNullId()
    {
        var ex = new EntityNotFoundException(typeof(int), null);

        ex.EntityId.Should().BeNull();
        ex.Message.Should().Contain("''");
    }

    [Fact]
    public void For_CreatesTypedException()
    {
        var id = 42;
        var ex = EntityNotFoundException.For<string>(id);

        ex.EntityType.Should().Be(typeof(string));
        ex.EntityId.Should().Be(id);
    }

    [Fact]
    public void InheritsFromDomainException()
    {
        var ex = new EntityNotFoundException(typeof(object), null);

        ex.Should().BeAssignableTo<DomainException>();
    }
}

public class ForbiddenExceptionTests
{
    [Fact]
    public void Constructor_SetsMessage()
    {
        var ex = new ForbiddenException("not allowed");

        ex.Message.Should().Be("not allowed");
    }

    [Fact]
    public void InheritsFromDomainException()
    {
        var ex = new ForbiddenException("nope");

        ex.Should().BeAssignableTo<DomainException>();
    }
}
