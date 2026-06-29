using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutritionService.Domain.Entities;

namespace NutritionService.Infrastructure.Persistence.Configurations;

public class MealPlanItemConfiguration : IEntityTypeConfiguration<MealPlanItem>
{
    public void Configure(EntityTypeBuilder<MealPlanItem> builder)
    {
        builder.ToTable("MealPlanItems");
        builder.HasKey(mpi => mpi.Id);
        builder.Property(mpi => mpi.Id).ValueGeneratedOnAdd();
        builder.Property(mpi => mpi.DayOfWeek).HasMaxLength(20).IsRequired();
        builder.Property(mpi => mpi.MealTime).HasMaxLength(50).IsRequired();

        builder.HasOne<MealPlan>()
            .WithMany()
            .HasForeignKey(mpi => mpi.MealPlanId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<Meal>()
            .WithMany()
            .HasForeignKey(mpi => mpi.MealId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
