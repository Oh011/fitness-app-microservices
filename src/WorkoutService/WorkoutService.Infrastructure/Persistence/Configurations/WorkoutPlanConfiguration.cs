using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutService.Domain.Entities;
using WorkoutService.Domain.Enums;

namespace WorkoutService.Infrastructure.Persistence.Configurations;

public class WorkoutPlanConfiguration : IEntityTypeConfiguration<WorkoutPlan>
{
    public void Configure(EntityTypeBuilder<WorkoutPlan> builder)
    {
        builder.ToTable("WorkoutPlans");
        builder.HasKey(wp => wp.Id);

        builder.Property(wp => wp.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(wp => wp.Description)
            .HasMaxLength(1000);

        builder.Property(wp => wp.DifficultyLevel)
            .HasConversion<int>();

        builder.Property(wp => wp.DurationWeeks);

        builder.Property(wp => wp.IsCustom);

        builder.Property(wp => wp.CreatedById);

        builder.Property(wp => wp.CreatedAtUtc);
    }
}
