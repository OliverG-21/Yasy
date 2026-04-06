using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yasy.Core.Entities;

namespace Yasy.Infrastructure.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customers");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasDefaultValueSql("gen_random_uuid()");

        builder.Property(c => c.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(c => c.LastName).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Street).HasMaxLength(200);
        builder.Property(c => c.ZipCode).HasMaxLength(10);
        builder.Property(c => c.City).HasMaxLength(100);
        builder.Property(c => c.Phone).HasMaxLength(30);
        builder.Property(c => c.Email).HasMaxLength(200);

        builder.Property(c => c.Birthday);
        builder.Property(c => c.InitialRegistration);
        builder.Property(c => c.ProfileImagePath).HasMaxLength(500);
        builder.Property(c => c.CreatedAt).HasDefaultValueSql("now()");

        builder.HasIndex(c => new { c.LastName, c.FirstName });
    }
}
