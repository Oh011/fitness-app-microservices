using FCEService.Domain.Entities;
using FCEService.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCEService.Infrastructure.Persistence.Configurations;

public class CalculatedMetricsConfiguration : IEntityTypeConfiguration<CalculatedMetrics>
{
    public void Configure(EntityTypeBuilder<CalculatedMetrics> builder)
    {
        builder.ToTable("CalculatedMetrics");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(c => c.BMR)
            .HasColumnType("float")
            .IsRequired();

        builder.Property(c => c.TDEE)
            .HasColumnType("float")
            .IsRequired();

        builder.Property(c => c.CalorieTarget)
            .HasColumnType("float")
            .IsRequired();

        builder.Property(c => c.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(c => c.CalculatedAt)
            .IsRequired();
    }
}
