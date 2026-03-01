using FluentAssertions;
using Seedwork.Exceptions;
using Seedwork.Tests.TestFixtures;

namespace Seedwork.Tests.Domain;

public class IdTests
{
    [Fact]
    public void New_CreatesNonEmptyGuid()
    {
        var id = TestId.New();

        id.Value.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void From_CreatesFromExistingGuid()
    {
        var guid = Guid.NewGuid();
        var id = TestId.From(guid);

        id.Value.Should().Be(guid);
    }

    [Fact]
    public void From_EmptyGuid_Throws()
    {
        var act = () => TestId.From(Guid.Empty);

        act.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void Parse_CreatesFromString()
    {
        var guid = Guid.NewGuid();
        var id = TestId.Parse(guid.ToString());

        id.Value.Should().Be(guid);
    }

    [Fact]
    public void Parse_InvalidString_Throws()
    {
        var act = () => TestId.Parse("not-a-guid");

        act.Should().Throw<FormatException>();
    }

    [Fact]
    public void ImplicitConversion_ToGuid()
    {
        var guid = Guid.NewGuid();
        var id = TestId.From(guid);

        Guid result = id;

        result.Should().Be(guid);
    }

    [Fact]
    public void ExplicitConversion_FromGuid()
    {
        var guid = Guid.NewGuid();

        var id = (TestId)(Seedwork.Domain.Id<TestId>)guid;

        id.Value.Should().Be(guid);
    }

    [Fact]
    public void Ids_WithSameValue_AreEqual()
    {
        var guid = Guid.NewGuid();
        var a = TestId.From(guid);
        var b = TestId.From(guid);

        a.Should().Be(b);
    }

    [Fact]
    public void Ids_WithDifferentValues_AreNotEqual()
    {
        var a = TestId.New();
        var b = TestId.New();

        a.Should().NotBe(b);
    }

    [Fact]
    public void ToString_ReturnsGuidString()
    {
        var guid = Guid.NewGuid();
        var id = TestId.From(guid);

        id.ToString().Should().Be(guid.ToString());
    }
}
