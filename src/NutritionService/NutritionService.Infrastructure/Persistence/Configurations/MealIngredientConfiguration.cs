using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutritionService.Domain.Entities;

namespace NutritionService.Infrastructure.Persistence.Configurations;

public class MealIngredientConfiguration : IEntityTypeConfiguration<MealIngredient>
{
    public void Configure(EntityTypeBuilder<MealIngredient> builder)
    {
        builder.ToTable("MealIngredients");
        builder.HasKey(mi => mi.Id);
        builder.Property(mi => mi.Id).ValueGeneratedOnAdd();
        builder.Property(mi => mi.Amount).HasMaxLength(100).IsRequired();

        builder.HasOne<Meal>()
            .WithMany()
            .HasForeignKey(mi => mi.MealId);

        builder.HasOne<Ingredient>()
            .WithMany()
            .HasForeignKey(mi => mi.IngredientId);
    }
}
