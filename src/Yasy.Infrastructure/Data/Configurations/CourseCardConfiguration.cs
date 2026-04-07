using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yasy.Core.Entities;

namespace Yasy.Infrastructure.Data.Configurations;

public class CourseCardConfiguration : IEntityTypeConfiguration<CourseCard>
{
    public void Configure(EntityTypeBuilder<CourseCard> builder)
    {
        builder.ToTable("course_cards");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasDefaultValueSql("gen_random_uuid()");

        builder.Property(c => c.CustomerId).IsRequired();
        builder.Property(c => c.Type).IsRequired();
        builder.Property(c => c.TotalVisits).IsRequired();
        builder.Property(c => c.UsedVisits).IsRequired().HasDefaultValue(0);
        builder.Property(c => c.PricePaid).HasPrecision(10, 2).IsRequired();
        builder.Property(c => c.PurchasedAt).IsRequired();
        builder.Property(c => c.ExhaustedAt);
        builder.Property(c => c.CreatedAt).HasDefaultValueSql("now()");

        builder.Ignore(c => c.RemainingVisits);

        builder.HasOne(c => c.Customer)
            .WithMany(cu => cu.Cards)
            .HasForeignKey(c => c.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => c.CustomerId);
    }
}
