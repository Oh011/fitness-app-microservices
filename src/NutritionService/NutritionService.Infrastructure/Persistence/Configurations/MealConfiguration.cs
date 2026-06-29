using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutritionService.Domain.Entities;

namespace NutritionService.Infrastructure.Persistence.Configurations;

public class MealConfiguration : IEntityTypeConfiguration<Meal>
{
    public void Configure(EntityTypeBuilder<Meal> builder)
    {
        builder.ToTable("Meals");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).ValueGeneratedOnAdd();
        builder.Property(m => m.Name).HasMaxLength(200).IsRequired();
        builder.Property(m => m.MealType).HasMaxLength(50).IsRequired();
        builder.Property(m => m.Difficulty).HasMaxLength(50).IsRequired();
        builder.Property(m => m.ImageUrl).HasMaxLength(500);
        builder.Property(m => m.IngredientsJson).HasColumnType("nvarchar(max)");
        builder.Property(m => m.InstructionsJson).HasColumnType("nvarchar(max)");
        builder.Property(m => m.VariationsJson).HasColumnType("nvarchar(max)");
        builder.Property(m => m.AllergensJson).HasMaxLength(500);
        builder.Property(m => m.TagsJson).HasMaxLength(500);

        builder.HasOne<MealPlan>()
            .WithMany()
            .HasForeignKey(m => m.MealPlanId);
    }
}
