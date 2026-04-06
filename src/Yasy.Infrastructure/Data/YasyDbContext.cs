using Microsoft.EntityFrameworkCore;
using Yasy.Core.Entities;

namespace Yasy.Infrastructure.Data;

public class YasyDbContext : DbContext
{
    public YasyDbContext(DbContextOptions<YasyDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(YasyDbContext).Assembly);
    }
}
