using FCEService.Domain.Entities;
using FCEService.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCEService.Infrastructure.Persistence.Configurations;

public class UserFitnessStatsConfiguration : IEntityTypeConfiguration<UserFitnessStats>
{
    public void Configure(EntityTypeBuilder<UserFitnessStats> builder)
    {
        builder.ToTable("UserFitnessStats");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(u => u.Weight)
            .HasColumnType("float")
            .IsRequired();

        builder.Property(u => u.Height)
            .HasColumnType("float")
            .IsRequired();

        builder.Property(u => u.Age)
            .IsRequired();

        builder.Property(u => u.Gender)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(u => u.Goal)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(u => u.ActivityLevel)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(u => u.RecordedAt)
            .IsRequired();
    }
}
