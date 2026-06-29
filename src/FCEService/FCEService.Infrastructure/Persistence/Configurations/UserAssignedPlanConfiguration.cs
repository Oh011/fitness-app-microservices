using FCEService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCEService.Infrastructure.Persistence.Configurations;

public class UserAssignedPlanConfiguration : IEntityTypeConfiguration<UserAssignedPlan>
{
    public void Configure(EntityTypeBuilder<UserAssignedPlan> builder)
    {
        builder.ToTable("UserAssignedPlans");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(u => u.ExternalPlanId)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.AssignedAt)
            .IsRequired();

        builder.Property(u => u.IsActive)
            .IsRequired();
    }
}
