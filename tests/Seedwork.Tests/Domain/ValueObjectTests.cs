using FluentAssertions;
using Seedwork.Tests.TestFixtures;

namespace Seedwork.Tests.Domain;

public class ValueObjectTests
{
    [Fact]
    public void ValueObjects_WithSameComponents_AreEqual()
    {
        var a = new Address("123 Main", "Springfield", "62701");
        var b = new Address("123 Main", "Springfield", "62701");

        a.Should().Be(b);
        (a == b).Should().BeTrue();
        (a != b).Should().BeFalse();
    }

    [Fact]
    public void ValueObjects_WithDifferentComponents_AreNotEqual()
    {
        var a = new Address("123 Main", "Springfield", "62701");
        var b = new Address("456 Oak", "Springfield", "62701");

        a.Should().NotBe(b);
        (a != b).Should().BeTrue();
    }

    [Fact]
    public void ValueObjects_OfDifferentTypes_AreNotEqual()
    {
        var address = new Address("123", "City", "00000");
        var money = new Money(123m, "USD");

        address.Equals(money).Should().BeFalse();
    }

    [Fact]
    public void ValueObject_ComparedToNull_IsNotEqual()
    {
        var address = new Address("123 Main", "City", "00000");

        address.Equals(null).Should().BeFalse();
        (address == null).Should().BeFalse();
        (null == address).Should().BeFalse();
    }

    [Fact]
    public void ValueObject_ComparedToSelf_IsEqual()
    {
        var address = new Address("123 Main", "City", "00000");

        address.Equals(address).Should().BeTrue();
    }

    [Fact]
    public void GetHashCode_IsSameForEqualValueObjects()
    {
        var a = new Address("123 Main", "City", "00000");
        var b = new Address("123 Main", "City", "00000");

        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void GetCopy_ReturnsEqualButSeparateInstance()
    {
        var original = new Address("123 Main", "City", "00000");
        var copy = original.GetCopy();

        copy.Should().Be(original);
        copy.Should().NotBeSameAs(original);
    }
}
