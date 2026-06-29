using FCEService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCEService.Infrastructure.Persistence.Configurations;

public class UserPlanHistoryConfiguration : IEntityTypeConfiguration<UserPlanHistory>
{
    public void Configure(EntityTypeBuilder<UserPlanHistory> builder)
    {
        builder.ToTable("UserPlanHistory");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(u => u.UserId)
            .IsRequired();

        builder.Property(u => u.ExternalPlanId)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.AssignedAt)
            .IsRequired();

        builder.Property(u => u.EndedAt)
            .IsRequired(false);

        builder.Property(u => u.ReasonForChange)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.HasIndex(u => u.UserId);
    }
}
