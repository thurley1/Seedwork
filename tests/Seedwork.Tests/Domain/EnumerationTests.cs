using FluentAssertions;
using Seedwork.Tests.TestFixtures;

namespace Seedwork.Tests.Domain;

public class EnumerationTests
{
    [Fact]
    public void GetAll_ReturnsAllMembers()
    {
        var all = OrderStatus.GetAll();

        all.Should().HaveCount(4);
        all.Should().Contain(OrderStatus.Pending);
        all.Should().Contain(OrderStatus.Confirmed);
        all.Should().Contain(OrderStatus.Shipped);
        all.Should().Contain(OrderStatus.Cancelled);
    }

    [Fact]
    public void FromValue_ReturnsCorrectMember()
    {
        var result = OrderStatus.FromValue(1);

        result.Should().BeSameAs(OrderStatus.Confirmed);
    }

    [Fact]
    public void FromValue_InvalidValue_Throws()
    {
        var act = () => OrderStatus.FromValue(99);

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void FromName_ReturnsCorrectMember_CaseInsensitive()
    {
        var result = OrderStatus.FromName("pending");

        result.Should().BeSameAs(OrderStatus.Pending);
    }

    [Fact]
    public void FromName_InvalidName_Throws()
    {
        var act = () => OrderStatus.FromName("Unknown");

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void TryFromValue_ReturnsTrue_WhenFound()
    {
        var found = OrderStatus.TryFromValue(2, out var result);

        found.Should().BeTrue();
        result.Should().BeSameAs(OrderStatus.Shipped);
    }

    [Fact]
    public void TryFromValue_ReturnsFalse_WhenNotFound()
    {
        var found = OrderStatus.TryFromValue(99, out var result);

        found.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void TryFromName_ReturnsTrue_WhenFound()
    {
        var found = OrderStatus.TryFromName("Cancelled", out var result);

        found.Should().BeTrue();
        result.Should().BeSameAs(OrderStatus.Cancelled);
    }

    [Fact]
    public void TryFromName_ReturnsFalse_WhenNotFound()
    {
        var found = OrderStatus.TryFromName("Nope", out var result);

        found.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void Equality_BasedOnValue()
    {
        var a = OrderStatus.FromValue(0);
        var b = OrderStatus.Pending;

        a.Should().Be(b);
        (a == b).Should().BeTrue();
    }

    [Fact]
    public void CompareTo_OrdersByValue()
    {
        OrderStatus.Pending.CompareTo(OrderStatus.Shipped).Should().BeNegative();
        OrderStatus.Shipped.CompareTo(OrderStatus.Pending).Should().BePositive();
        OrderStatus.Confirmed.CompareTo(OrderStatus.Confirmed).Should().Be(0);
    }

    [Fact]
    public void ToString_ReturnsName()
    {
        OrderStatus.Confirmed.ToString().Should().Be("Confirmed");
    }
}
