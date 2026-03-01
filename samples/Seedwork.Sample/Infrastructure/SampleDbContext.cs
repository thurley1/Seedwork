using Microsoft.EntityFrameworkCore;
using Seedwork.EntityFrameworkCore.Configuration;
using Seedwork.Sample.Domain.Orders;

namespace Seedwork.Sample.Infrastructure;

public class SampleDbContext : DbContext
{
    public DbSet<Order> Orders => Set<Order>();

    public SampleDbContext(DbContextOptions<SampleDbContext> options) : base(options) { }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.ConfigureIdConventions();
        configurationBuilder.ConfigureEnumerationConventions<OrderStatus>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(b =>
        {
            b.HasKey(o => o.Id);
            b.Property(o => o.CustomerName).HasMaxLength(200);
            b.OwnsMany(o => o.Items, items =>
            {
                items.WithOwner().HasForeignKey("OrderId");
                items.Property(i => i.ProductName).HasMaxLength(200);
                items.Ignore(i => i.LineTotal);
                items.OwnsOne(i => i.UnitPrice);
            });
        });

        modelBuilder.IgnoreDomainEvents();
    }
}
