using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutritionService.Domain.Entities;

namespace NutritionService.Infrastructure.Persistence.Configurations;

public class NutritionFactConfiguration : IEntityTypeConfiguration<NutritionFact>
{
    public void Configure(EntityTypeBuilder<NutritionFact> builder)
    {
        builder.ToTable("NutritionFacts");
        builder.HasKey(nf => nf.Id);
        builder.Property(nf => nf.Id).ValueGeneratedNever(); // 1-to-1 with Meals
        builder.Property(nf => nf.Protein).HasColumnType("float");
        builder.Property(nf => nf.Carbs).HasColumnType("float");
        builder.Property(nf => nf.Fats).HasColumnType("float");
        builder.Property(nf => nf.Fiber).HasColumnType("float");

        builder.HasOne<Meal>()
            .WithOne()
            .HasForeignKey<NutritionFact>(nf => nf.Id);
    }
}
