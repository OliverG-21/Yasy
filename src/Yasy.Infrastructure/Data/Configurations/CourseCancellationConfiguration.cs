using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yasy.Core.Entities;

namespace Yasy.Infrastructure.Data.Configurations;

public class CourseCancellationConfiguration : IEntityTypeConfiguration<CourseCancellation>
{
    public void Configure(EntityTypeBuilder<CourseCancellation> builder)
    {
        builder.ToTable("course_cancellations");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasDefaultValueSql("gen_random_uuid()");

        builder.Property(c => c.CourseId).IsRequired();
        builder.Property(c => c.Date).IsRequired();
        builder.Property(c => c.CreatedAt).HasDefaultValueSql("now()");

        builder.HasIndex(c => new { c.CourseId, c.Date }).IsUnique();
    }
}
