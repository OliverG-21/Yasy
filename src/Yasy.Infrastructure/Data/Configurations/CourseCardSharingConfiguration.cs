using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yasy.Core.Entities;

namespace Yasy.Infrastructure.Data.Configurations;

public class CourseCardSharingConfiguration : IEntityTypeConfiguration<CourseCardSharing>
{
    public void Configure(EntityTypeBuilder<CourseCardSharing> builder)
    {
        builder.ToTable("course_card_sharings");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasDefaultValueSql("gen_random_uuid()");

        builder.Property(s => s.CourseCardId).IsRequired();
        builder.Property(s => s.CustomerId).IsRequired();
        builder.Property(s => s.CreatedAt).HasDefaultValueSql("now()");

        builder.HasOne(s => s.CourseCard)
            .WithMany(c => c.SharedWith)
            .HasForeignKey(s => s.CourseCardId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Customer)
            .WithMany()
            .HasForeignKey(s => s.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => new { s.CourseCardId, s.CustomerId }).IsUnique();
    }
}
