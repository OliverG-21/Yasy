using Microsoft.EntityFrameworkCore;

namespace Yasy.Infrastructure.Data;

public class YasyDbContext : DbContext
{
    public YasyDbContext(DbContextOptions<YasyDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(YasyDbContext).Assembly);
    }
}
