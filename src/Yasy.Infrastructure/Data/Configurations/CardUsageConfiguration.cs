using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yasy.Core.Entities;

namespace Yasy.Infrastructure.Data.Configurations;

public class CardUsageConfiguration : IEntityTypeConfiguration<CardUsage>
{
    public void Configure(EntityTypeBuilder<CardUsage> builder)
    {
        builder.ToTable("card_usages");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasDefaultValueSql("gen_random_uuid()");

        builder.Property(u => u.CourseCardId).IsRequired();
        builder.Property(u => u.CourseId);
        builder.Property(u => u.UsedByCustomerId);
        builder.Property(u => u.UsedAt).IsRequired();
        builder.Property(u => u.CreatedAt).HasDefaultValueSql("now()");

        builder.HasOne(u => u.CourseCard)
            .WithMany(c => c.Usages)
            .HasForeignKey(u => u.CourseCardId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(u => u.Course)
            .WithMany()
            .HasForeignKey(u => u.CourseId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(u => u.UsedByCustomer)
            .WithMany()
            .HasForeignKey(u => u.UsedByCustomerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(u => u.CourseCardId);
    }
}
