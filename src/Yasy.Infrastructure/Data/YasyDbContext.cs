using Microsoft.EntityFrameworkCore;
using Yasy.Core.Entities;

namespace Yasy.Infrastructure.Data;

public class YasyDbContext : DbContext
{
    public YasyDbContext(DbContextOptions<YasyDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<CourseCancellation> CourseCancellations => Set<CourseCancellation>();
    public DbSet<CourseEnrollment> CourseEnrollments => Set<CourseEnrollment>();
    public DbSet<CourseCard> CourseCards => Set<CourseCard>();
    public DbSet<CardUsage> CardUsages => Set<CardUsage>();
    public DbSet<CourseCardSharing> CourseCardSharings => Set<CourseCardSharing>();
    public DbSet<AttendanceRecord> AttendanceRecords => Set<AttendanceRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(YasyDbContext).Assembly);
    }
}
