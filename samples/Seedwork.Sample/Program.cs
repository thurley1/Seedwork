using Microsoft.EntityFrameworkCore;
using Seedwork.EntityFrameworkCore.Interceptors;
using Seedwork.Sample.Domain.Orders;
using Seedwork.Sample.Infrastructure;

// Set up domain event dispatch
var interceptor = new DomainEventDispatchInterceptor(async (evt, _) =>
{
    Console.WriteLine($"  [Event] {evt.GetType().Name} at {evt.OccurredOnUtc:HH:mm:ss.fff}");
    await Task.CompletedTask;
});

// Configure EF Core with InMemory provider
var options = new DbContextOptionsBuilder<SampleDbContext>()
    .UseInMemoryDatabase("SeedworkSample")
    .AddInterceptors(interceptor)
    .Options;

// Create an order
Console.WriteLine("Creating order...");
var order = Order.Create("Jane Smith");
order.AddItem("Mechanical Keyboard", 1, new Money(149.99m, "USD"));
order.AddItem("USB-C Cable", 3, new Money(12.99m, "USD"));

// Persist
await using (var ctx = new SampleDbContext(options))
{
    ctx.Orders.Add(order);
    Console.WriteLine("Saving changes...");
    await ctx.SaveChangesAsync();
}

// Read back
await using (var ctx = new SampleDbContext(options))
{
    var loaded = await ctx.Orders
        .Include("Items")
        .FirstAsync(o => o.Id == order.Id);

    Console.WriteLine($"\nOrder: {loaded}");
    Console.WriteLine($"  Customer: {loaded.CustomerName}");
    Console.WriteLine($"  Status: {loaded.Status}");
    Console.WriteLine($"  Items: {loaded.Items.Count}");
    foreach (var item in loaded.Items)
    {
        Console.WriteLine($"    - {item.ProductName} x{item.Quantity} @ {item.UnitPrice.Amount} {item.UnitPrice.Currency}");
    }
}

Console.WriteLine("\nDone!");
