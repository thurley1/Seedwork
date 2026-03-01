using FluentAssertions;
using Seedwork.Exceptions;
using Seedwork.Guard;

namespace Seedwork.Tests.Guard;

public class DomainGuardTests
{
    [Fact]
    public void AgainstNull_WithValue_ReturnsValue()
    {
        var result = DomainGuard.AgainstNull("hello", "param");
        result.Should().Be("hello");
    }

    [Fact]
    public void AgainstNull_WithNull_Throws()
    {
        var act = () => DomainGuard.AgainstNull<string>(null, "param");
        act.Should().Throw<DomainValidationException>()
            .Which.ParameterName.Should().Be("param");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void AgainstNullOrWhiteSpace_WithInvalidValue_Throws(string? value)
    {
        var act = () => DomainGuard.AgainstNullOrWhiteSpace(value, "name");
        act.Should().Throw<DomainValidationException>()
            .Which.ParameterName.Should().Be("name");
    }

    [Fact]
    public void AgainstNullOrWhiteSpace_WithValue_ReturnsValue()
    {
        var result = DomainGuard.AgainstNullOrWhiteSpace("hello", "name");
        result.Should().Be("hello");
    }

    [Fact]
    public void AgainstEmpty_WithEmptyGuid_Throws()
    {
        var act = () => DomainGuard.AgainstEmpty(Guid.Empty, "id");
        act.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void AgainstEmpty_WithValidGuid_ReturnsValue()
    {
        var guid = Guid.NewGuid();
        var result = DomainGuard.AgainstEmpty(guid, "id");
        result.Should().Be(guid);
    }

    [Fact]
    public void AgainstLessThan_WhenBelow_Throws()
    {
        var act = () => DomainGuard.AgainstLessThan(5m, 10m, "price");
        act.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void AgainstLessThan_WhenEqual_ReturnsValue()
    {
        var result = DomainGuard.AgainstLessThan(10m, 10m, "price");
        result.Should().Be(10m);
    }

    [Fact]
    public void AgainstOutOfRange_WhenOutside_Throws()
    {
        var act = () => DomainGuard.AgainstOutOfRange(11, 1, 10, "qty");
        act.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void AgainstOutOfRange_WhenInside_ReturnsValue()
    {
        var result = DomainGuard.AgainstOutOfRange(5, 1, 10, "qty");
        result.Should().Be(5);
    }

    [Fact]
    public void AgainstOutOfRange_AtBoundaries_ReturnsValue()
    {
        DomainGuard.AgainstOutOfRange(1, 1, 10, "qty").Should().Be(1);
        DomainGuard.AgainstOutOfRange(10, 1, 10, "qty").Should().Be(10);
    }

    [Fact]
    public void AgainstNegative_WhenNegative_Throws()
    {
        var act = () => DomainGuard.AgainstNegative(-1m, "amount");
        act.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void AgainstNegative_WhenZero_ReturnsValue()
    {
        DomainGuard.AgainstNegative(0m, "amount").Should().Be(0m);
    }

    [Fact]
    public void AgainstNegativeOrZero_WhenZero_Throws()
    {
        var act = () => DomainGuard.AgainstNegativeOrZero(0m, "amount");
        act.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void AgainstNegativeOrZero_WhenPositive_ReturnsValue()
    {
        DomainGuard.AgainstNegativeOrZero(1m, "amount").Should().Be(1m);
    }

    [Fact]
    public void AgainstInvalidFormat_WhenNoMatch_Throws()
    {
        var act = () => DomainGuard.AgainstInvalidFormat("abc", @"^\d+$", "code");
        act.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void AgainstInvalidFormat_WhenMatches_ReturnsValue()
    {
        var result = DomainGuard.AgainstInvalidFormat("123", @"^\d+$", "code");
        result.Should().Be("123");
    }

    [Fact]
    public void AgainstLength_WhenExceeds_Throws()
    {
        var act = () => DomainGuard.AgainstLength("hello world", 5, "title");
        act.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void AgainstLength_WhenWithin_ReturnsValue()
    {
        var result = DomainGuard.AgainstLength("hello", 10, "title");
        result.Should().Be("hello");
    }

    [Fact]
    public void AgainstLengthOutOfRange_WhenTooShort_Throws()
    {
        var act = () => DomainGuard.AgainstLengthOutOfRange("ab", 3, 10, "name");
        act.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void AgainstLengthOutOfRange_WhenTooLong_Throws()
    {
        var act = () => DomainGuard.AgainstLengthOutOfRange("hello world!", 3, 10, "name");
        act.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void AgainstLengthOutOfRange_WhenInRange_ReturnsValue()
    {
        var result = DomainGuard.AgainstLengthOutOfRange("hello", 3, 10, "name");
        result.Should().Be("hello");
    }

    [Fact]
    public void AgainstEmptyCollection_WhenEmpty_Throws()
    {
        var act = () => DomainGuard.AgainstEmptyCollection(Array.Empty<int>(), "items");
        act.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void AgainstEmptyCollection_WhenHasElements_ReturnsValue()
    {
        var items = new[] { 1, 2, 3 };
        var result = DomainGuard.AgainstEmptyCollection(items, "items");
        result.Should().BeEquivalentTo(items);
    }

    [Fact]
    public void AgainstInvalidEmail_WhenInvalid_Throws()
    {
        var act = () => DomainGuard.AgainstInvalidEmail("not-an-email", "email");
        act.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void AgainstInvalidEmail_WhenValid_ReturnsValue()
    {
        var result = DomainGuard.AgainstInvalidEmail("user@example.com", "email");
        result.Should().Be("user@example.com");
    }

    [Theory]
    [InlineData("user@domain.com")]
    [InlineData("user.name@domain.co.uk")]
    [InlineData("user+tag@domain.org")]
    public void AgainstInvalidEmail_AcceptsValidFormats(string email)
    {
        var result = DomainGuard.AgainstInvalidEmail(email, "email");
        result.Should().Be(email);
    }

    [Theory]
    [InlineData("@domain.com")]
    [InlineData("user@")]
    [InlineData("user@domain")]
    [InlineData("user domain.com")]
    public void AgainstInvalidEmail_RejectsInvalidFormats(string email)
    {
        var act = () => DomainGuard.AgainstInvalidEmail(email, "email");
        act.Should().Throw<DomainValidationException>();
    }
}
