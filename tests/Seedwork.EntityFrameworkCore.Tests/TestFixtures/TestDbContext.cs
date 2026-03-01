using Microsoft.EntityFrameworkCore;
using Seedwork.EntityFrameworkCore.Configuration;

namespace Seedwork.EntityFrameworkCore.Tests.TestFixtures;

public class TestDbContext : DbContext
{
    public DbSet<Product> Products => Set<Product>();

    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.ConfigureIdConventions();
        configurationBuilder.ConfigureEnumerationConventions<ProductStatus>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(b =>
        {
            b.HasKey(p => p.Id);
            b.Property(p => p.Name).HasMaxLength(200);
        });

        modelBuilder.IgnoreDomainEvents();
    }
}
