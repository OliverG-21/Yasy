using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yasy.Core.Entities;

namespace Yasy.Infrastructure.Data.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("courses");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasDefaultValueSql("gen_random_uuid()");

        builder.Property(c => c.Name).HasMaxLength(200).IsRequired();
        builder.Property(c => c.Type).IsRequired();

        // Workshop fields
        builder.Property(c => c.Date);
        builder.Property(c => c.StartTime);
        builder.Property(c => c.EndTime);
        builder.Property(c => c.Price).HasPrecision(10, 2);

        // Regular course fields
        builder.Property(c => c.DayOfWeek);
        builder.Property(c => c.DurationMinutes);
        builder.Property(c => c.DropInPrice).HasPrecision(10, 2);
        builder.Property(c => c.FiveCardPrice).HasPrecision(10, 2);
        builder.Property(c => c.TenCardPrice).HasPrecision(10, 2);

        builder.Property(c => c.CreatedAt).HasDefaultValueSql("now()");

        builder.HasIndex(c => c.Type);
        builder.HasIndex(c => c.Name);
    }
}
