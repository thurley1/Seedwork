using FluentAssertions;
using Seedwork.EntityFrameworkCore.Converters;
using Seedwork.EntityFrameworkCore.Tests.TestFixtures;

namespace Seedwork.EntityFrameworkCore.Tests.Converters;

public class IdValueConverterTests
{
    [Fact]
    public void ConvertToProvider_ReturnsGuid()
    {
        var converter = new IdValueConverter<ProductId>();
        var id = ProductId.New();

        var result = converter.ConvertToProvider(id);

        result.Should().Be(id.Value);
    }

    [Fact]
    public void ConvertFromProvider_ReturnsId()
    {
        var converter = new IdValueConverter<ProductId>();
        var guid = Guid.NewGuid();

        var result = converter.ConvertFromProvider(guid);

        result.Should().NotBeNull();
        ((ProductId)result!).Value.Should().Be(guid);
    }
}

public class EnumerationValueConverterTests
{
    [Fact]
    public void ConvertToProvider_ReturnsInt()
    {
        var converter = new EnumerationValueConverter<ProductStatus>();

        var result = converter.ConvertToProvider(ProductStatus.Active);

        result.Should().Be(1);
    }

    [Fact]
    public void ConvertFromProvider_ReturnsEnumeration()
    {
        var converter = new EnumerationValueConverter<ProductStatus>();

        var result = converter.ConvertFromProvider(2);

        result.Should().Be(ProductStatus.Discontinued);
    }
}
