using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutService.Domain.Entities;

namespace WorkoutService.Infrastructure.Persistence.Configurations;

public class WorkoutExerciseConfiguration : IEntityTypeConfiguration<WorkoutExercise>
{
    public void Configure(EntityTypeBuilder<WorkoutExercise> builder)
    {
        builder.ToTable("WorkoutExercises");
        builder.HasKey(we => we.Id);

        builder.Property(we => we.Sets);
        builder.Property(we => we.Reps);
        builder.Property(we => we.RestSeconds);
        builder.Property(we => we.SortOrder);

        builder.HasOne<Workout>()
            .WithMany()
            .HasForeignKey(we => we.WorkoutId);

        builder.HasOne<Exercise>()
            .WithMany()
            .HasForeignKey(we => we.ExerciseId);
    }
}
