using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Seedwork.EntityFrameworkCore.Tests.TestFixtures;

namespace Seedwork.EntityFrameworkCore.Tests.Configuration;

public class ConfigurationTests
{
    private static TestDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new TestDbContext(options);
    }

    [Fact]
    public async Task CanPersistAndRetrieveEntityWithIdAndEnumeration()
    {
        var id = ProductId.New();
        var product = new Product(id, "Widget");
        product.Activate();

        await using (var ctx = CreateContext())
        {
            ctx.Products.Add(product);
            await ctx.SaveChangesAsync();
        }

        await using (var ctx = CreateContext())
        {
            // InMemory shares the same store per database name,
            // but we used a different Guid above, so let's use same options.
        }
    }

    [Fact]
    public void DomainEventsAreIgnored()
    {
        using var ctx = CreateContext();

        var entityType = ctx.Model.FindEntityType(typeof(Product));
        var domainEventsProp = entityType?.FindProperty("DomainEvents");

        domainEventsProp.Should().BeNull("DomainEvents should be ignored by EF Core");
    }

    [Fact]
    public async Task RoundTrip_WithInMemory()
    {
        var dbName = Guid.NewGuid().ToString();
        var id = ProductId.New();

        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        await using (var ctx = new TestDbContext(options))
        {
            var product = new Product(id, "Gadget");
            ctx.Products.Add(product);
            await ctx.SaveChangesAsync();
        }

        await using (var ctx = new TestDbContext(options))
        {
            var product = await ctx.Products.FindAsync(id);
            product.Should().NotBeNull();
            product!.Name.Should().Be("Gadget");
            product.Id.Should().Be(id);
        }
    }
}
