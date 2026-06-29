using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutritionService.Domain.Entities;

namespace NutritionService.Infrastructure.Persistence.Configurations;

public class MealPlanConfiguration : IEntityTypeConfiguration<MealPlan>
{
    public void Configure(EntityTypeBuilder<MealPlan> builder)
    {
        builder.ToTable("MealPlans");
        builder.HasKey(mp => mp.Id);
        builder.Property(mp => mp.Id).ValueGeneratedOnAdd();
        builder.Property(mp => mp.Name).HasMaxLength(200).IsRequired();
        builder.Property(mp => mp.Description).HasMaxLength(500);
        builder.Property(mp => mp.CalorieTarget).IsRequired();
    }
}
