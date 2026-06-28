using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutService.Domain.Entities;
using WorkoutService.Domain.Enums;

namespace WorkoutService.Infrastructure.Persistence.Configurations;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.ToTable("Exercises");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        builder.Property(e => e.MuscleGroup)
            .HasMaxLength(100);

        builder.Property(e => e.EquipmentNeeded)
            .HasMaxLength(200);

        builder.Property(e => e.DifficultyLevel)
            .HasConversion<int>();

        builder.Property(e => e.VideoUrl)
            .HasMaxLength(500);

        builder.Property(e => e.ImageUrl)
            .HasMaxLength(500);

        builder.Property(e => e.IsActive);

        builder.Property(e => e.CreatedAtUtc);

        builder.HasIndex(e => e.MuscleGroup);
        builder.HasIndex(e => e.IsActive);
    }
}
