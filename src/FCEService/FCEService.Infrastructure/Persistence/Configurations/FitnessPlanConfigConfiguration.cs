using FCEService.Domain.Entities;
using FCEService.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCEService.Infrastructure.Persistence.Configurations;

public class FitnessPlanConfigConfiguration : IEntityTypeConfiguration<FitnessPlanConfig>
{
    public void Configure(EntityTypeBuilder<FitnessPlanConfig> builder)
    {
        builder.ToTable("FitnessPlanConfigs");
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(f => f.Goal)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(f => f.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(f => f.CalorieMin)
            .HasColumnType("float")
            .IsRequired();

        builder.Property(f => f.CalorieMax)
            .HasColumnType("float")
            .IsRequired();

        builder.Property(f => f.ExternalPlanId)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(f => f.PlanName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(f => f.WorkoutsPerWeek)
            .IsRequired();
    }
}
