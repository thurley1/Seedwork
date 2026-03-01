using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Seedwork.Abstractions;
using Seedwork.EntityFrameworkCore.Interceptors;
using Seedwork.EntityFrameworkCore.Tests.TestFixtures;

namespace Seedwork.EntityFrameworkCore.Tests.Interceptors;

public class DomainEventDispatchInterceptorTests
{
    [Fact]
    public async Task SaveChangesAsync_DispatchesDomainEvents()
    {
        var dispatched = new List<IDomainEvent>();
        var interceptor = new DomainEventDispatchInterceptor(
            (evt, _) =>
            {
                dispatched.Add(evt);
                return Task.CompletedTask;
            });

        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .AddInterceptors(interceptor)
            .Options;

        await using var ctx = new TestDbContext(options);
        var product = new Product(ProductId.New(), "Test Product");
        ctx.Products.Add(product);

        await ctx.SaveChangesAsync();

        dispatched.Should().HaveCount(1);
        dispatched[0].Should().BeOfType<ProductCreatedEvent>();
    }

    [Fact]
    public async Task SaveChangesAsync_ClearsEventsAfterDispatch()
    {
        var interceptor = new DomainEventDispatchInterceptor(
            (_, _) => Task.CompletedTask);

        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .AddInterceptors(interceptor)
            .Options;

        await using var ctx = new TestDbContext(options);
        var product = new Product(ProductId.New(), "Test Product");
        ctx.Products.Add(product);

        await ctx.SaveChangesAsync();

        product.DomainEvents.Should().BeEmpty();
    }
}
