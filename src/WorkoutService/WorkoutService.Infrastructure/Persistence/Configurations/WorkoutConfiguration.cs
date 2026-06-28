using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutService.Domain.Entities;
using WorkoutService.Domain.Enums;

namespace WorkoutService.Infrastructure.Persistence.Configurations;

public class WorkoutConfiguration : IEntityTypeConfiguration<Workout>
{
    public void Configure(EntityTypeBuilder<Workout> builder)
    {
        builder.ToTable("Workouts");
        builder.HasKey(w => w.Id);

        builder.Property(w => w.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(w => w.Description)
            .HasMaxLength(1000);

        builder.Property(w => w.WorkoutCategory)
            .HasConversion<int>();

        builder.Property(w => w.DifficultyLevel)
            .HasConversion<int>();

        builder.Property(w => w.DurationMinutes);

        builder.Property(w => w.CaloriesBurnedEstimate);

        builder.Property(w => w.IsActive);

        builder.Property(w => w.CreatedAtUtc);

        builder.HasOne<WorkoutPlan>()
            .WithMany()
            .HasForeignKey(w => w.PlanId);

        builder.HasIndex(w => w.WorkoutCategory);
        builder.HasIndex(w => w.IsActive);
    }
}
