using FluentAssertions;
using Seedwork.Tests.TestFixtures;

namespace Seedwork.Tests.Domain;

public class EntityTests
{
    [Fact]
    public void Entities_WithSameId_AreEqual()
    {
        var id = Guid.NewGuid();
        var a = new TestEntity(id, "A");
        var b = new TestEntity(id, "B");

        a.Should().Be(b);
        (a == b).Should().BeTrue();
        (a != b).Should().BeFalse();
    }

    [Fact]
    public void Entities_WithDifferentIds_AreNotEqual()
    {
        var a = new TestEntity(Guid.NewGuid(), "A");
        var b = new TestEntity(Guid.NewGuid(), "A");

        a.Should().NotBe(b);
        (a != b).Should().BeTrue();
    }

    [Fact]
    public void Entities_OfDifferentTypes_WithSameId_AreNotEqual()
    {
        var id = Guid.NewGuid();
        var entity = new TestEntity(id, "A");
        var other = new OtherTestEntity(id);

        entity.Equals(other).Should().BeFalse();
    }

    [Fact]
    public void Entity_ComparedToNull_IsNotEqual()
    {
        var entity = new TestEntity(Guid.NewGuid(), "A");

        entity.Equals(null).Should().BeFalse();
        (entity == null).Should().BeFalse();
        (null == entity).Should().BeFalse();
    }

    [Fact]
    public void Entity_ComparedToSelf_IsEqual()
    {
        var entity = new TestEntity(Guid.NewGuid(), "A");

        entity.Equals(entity).Should().BeTrue();
#pragma warning disable CS1718 // Comparison made to same variable
        (entity == entity).Should().BeTrue();
#pragma warning restore CS1718
    }

    [Fact]
    public void GetHashCode_IsSameForEqualEntities()
    {
        var id = Guid.NewGuid();
        var a = new TestEntity(id, "A");
        var b = new TestEntity(id, "B");

        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void ToString_ReturnsTypeNameAndId()
    {
        var id = Guid.NewGuid();
        var entity = new TestEntity(id, "A");

        entity.ToString().Should().Be($"TestEntity [{id}]");
    }
}
