using Microsoft.EntityFrameworkCore;
using NutritionService.Domain.Entities;

namespace NutritionService.Infrastructure.Persistence.Context;

public class NutritionDbContext : DbContext
{
    public NutritionDbContext(DbContextOptions<NutritionDbContext> options) : base(options) { }

    public DbSet<MealPlan> MealPlans { get; set; }
    public DbSet<Meal> Meals { get; set; }
    public DbSet<NutritionFact> NutritionFacts { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<MealIngredient> MealIngredients { get; set; }
    public DbSet<MealPlanItem> MealPlanItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NutritionDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
