using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutService.Domain.Entities;
using WorkoutService.Domain.Enums;

namespace WorkoutService.Infrastructure.Persistence.Configurations;

public class WorkoutSessionConfiguration : IEntityTypeConfiguration<WorkoutSession>
{
    public void Configure(EntityTypeBuilder<WorkoutSession> builder)
    {
        builder.ToTable("WorkoutSessions");
        builder.HasKey(ws => ws.Id);

        builder.Property(ws => ws.UserId);

        builder.Property(ws => ws.StartedAtUtc);

        builder.Property(ws => ws.CompletedAtUtc);

        builder.Property(ws => ws.Status)
            .HasConversion<int>();

        builder.Property(ws => ws.Notes)
            .HasMaxLength(2000);

        builder.HasOne<Workout>()
            .WithMany()
            .HasForeignKey(ws => ws.WorkoutId);

        builder.HasIndex(ws => new { ws.UserId, ws.Status });
    }
}
