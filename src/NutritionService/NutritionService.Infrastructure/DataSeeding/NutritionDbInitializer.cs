using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NutritionService.Application.Abstractions.Data;
using NutritionService.Domain.Entities;
using NutritionService.Infrastructure.Persistence.Context;

namespace NutritionService.Infrastructure.DataSeeding;

internal class NutritionDbInitializer : INutritionDbInitializer
{
    private readonly NutritionDbContext _context;
    private readonly ILogger<NutritionDbInitializer> _logger;

    public NutritionDbInitializer(NutritionDbContext context, ILogger<NutritionDbInitializer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        try
        {
            await _context.Database.EnsureCreatedAsync();

            // Seed in FK-safe order. NutritionFacts uses ValueGeneratedNever (1:1 with Meals),
            // so it has no identity column — skip IDENTITY_INSERT for it.
            if (!await _context.MealPlans.AnyAsync())
                await SeedTableAsync("MealPlans", hasIdentity: true, SeedMealPlansAsync);

            if (!await _context.Meals.AnyAsync())
                await SeedTableAsync("Meals", hasIdentity: true, SeedMealsAsync);

            if (!await _context.NutritionFacts.AnyAsync())
                await SeedTableAsync("NutritionFacts", hasIdentity: false, SeedNutritionFactsAsync);

            if (!await _context.Ingredients.AnyAsync())
                await SeedTableAsync("Ingredients", hasIdentity: true, SeedIngredientsAsync);

            if (!await _context.MealIngredients.AnyAsync())
                await SeedTableAsync("MealIngredients", hasIdentity: true, SeedMealIngredientsAsync);

            if (!await _context.MealPlanItems.AnyAsync())
                await SeedTableAsync("MealPlanItems", hasIdentity: true, SeedMealPlanItemsAsync);

            _logger.LogInformation("Nutrition database seeded successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the nutrition database.");
            throw;
        }
    }

    private async Task SeedTableAsync(string tableName, bool hasIdentity, Func<Task> seedAction)
    {
        // Open connection explicitly so SET IDENTITY_INSERT (per-session) persists
        // across ExecuteSqlRawAsync and SaveChangesAsync calls.
        await _context.Database.OpenConnectionAsync();
        try
        {
            if (hasIdentity)
            {
#pragma warning disable EF1002
                await _context.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {tableName} ON");
#pragma warning restore EF1002
            }

            await seedAction();
            await _context.SaveChangesAsync();

            if (hasIdentity)
            {
#pragma warning disable EF1002
                await _context.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {tableName} OFF");
#pragma warning restore EF1002
            }
        }
        finally
        {
            await _context.Database.CloseConnectionAsync();
        }
    }

    private async Task SeedMealPlansAsync()
    {
        var plans = new List<MealPlan>
        {
            new() { Id = 1, Name = "High Protein 2200 kcal", Description = "A plan for muscle gain.", CalorieTarget = 2200 },
            new() { Id = 2, Name = "Balanced 1800 kcal", Description = "A plan for weight maintenance.", CalorieTarget = 1800 },
            new() { Id = 3, Name = "Low Carb 1500 kcal", Description = "A plan for weight loss.", CalorieTarget = 1500 },
            new() { Id = 4, Name = "Vegan 2000 kcal", Description = "A plant-based nutrition plan.", CalorieTarget = 2000 },
        };
        await _context.MealPlans.AddRangeAsync(plans);
    }

    private async Task SeedMealsAsync()
    {
        var meals = new List<Meal>
        {
            new() { Id = 1, MealPlanId = 1, Name = "Grilled Chicken Lunch", MealType = "Lunch", PrepTimeInMinutes = 30, Difficulty = "Medium", ImageUrl = "/images/meals/1.jpg", IngredientsJson = "[\"Chicken Breast\", \"Olive Oil\", \"Mixed Herbs\"]", InstructionsJson = "[\"Season chicken with herbs.\", \"Grill for 6-7 mins each side.\", \"Serve with vegetables.\"]", AllergensJson = "[]", TagsJson = "[\"High Protein\", \"Keto\"]" },
            new() { Id = 2, MealPlanId = 1, Name = "Protein Oatmeal", MealType = "Breakfast", PrepTimeInMinutes = 10, Difficulty = "Easy", ImageUrl = "/images/meals/2.jpg", IngredientsJson = "[\"Oats\", \"Whey Protein\", \"Banana\"]", InstructionsJson = "[\"Cook oats with water.\", \"Mix in protein powder.\", \"Top with banana slices.\"]", AllergensJson = "[\"Dairy\"]", TagsJson = "[\"High Protein\", \"Quick\"]" },
            new() { Id = 3, MealPlanId = 2, Name = "Salmon Salad", MealType = "Lunch", PrepTimeInMinutes = 20, Difficulty = "Medium", ImageUrl = "/images/meals/3.jpg", IngredientsJson = "[\"Salmon\", \"Mixed Greens\", \"Lemon\"]", InstructionsJson = "[\"Pan-sear salmon.\", \"Toss greens with dressing.\", \"Top with salmon.\"]", AllergensJson = "[\"Fish\"]", TagsJson = "[\"Healthy\", \"Omega-3\"]" },
            new() { Id = 4, MealPlanId = 2, Name = "Greek Yogurt Bowl", MealType = "Breakfast", PrepTimeInMinutes = 5, Difficulty = "Easy", ImageUrl = "/images/meals/4.jpg", IngredientsJson = "[\"Greek Yogurt\", \"Honey\", \"Granola\"]", InstructionsJson = "[\"Add yogurt to bowl.\", \"Drizzle honey.\", \"Top with granola.\"]", AllergensJson = "[\"Dairy\"]", TagsJson = "[\"Quick\", \"High Protein\"]" },
            new() { Id = 5, MealPlanId = 3, Name = "Grilled Chicken Wrap", MealType = "Dinner", PrepTimeInMinutes = 25, Difficulty = "Easy", ImageUrl = "/images/meals/5.jpg", IngredientsJson = "[\"Chicken\", \"Lettuce\", \"Whole Wheat Wrap\"]", InstructionsJson = "[\"Grill chicken and slice.\", \"Assemble wrap with lettuce.\", \"Roll and serve.\"]", AllergensJson = "[\"Gluten\"]", TagsJson = "[\"Low Carb\", \"Quick\"]" },
            new() { Id = 6, MealPlanId = 4, Name = "Vegan Buddha Bowl", MealType = "Dinner", PrepTimeInMinutes = 35, Difficulty = "Medium", ImageUrl = "/images/meals/6.jpg", IngredientsJson = "[\"Quinoa\", \"Chickpeas\", \"Avocado\", \"Sweet Potato\"]", InstructionsJson = "[\"Cook quinoa.\", \"Roast sweet potato.\", \"Assemble bowl with all ingredients.\"]", AllergensJson = "[]", TagsJson = "[\"Vegan\", \"Healthy\"]" },
        };
        await _context.Meals.AddRangeAsync(meals);
    }

    private async Task SeedNutritionFactsAsync()
    {
        var facts = new List<NutritionFact>
        {
            new() { Id = 1, Calories = 650, Protein = 55.0, Carbs = 20.0, Fats = 35.0, Fiber = 5.0 },
            new() { Id = 2, Calories = 420, Protein = 35.0, Carbs = 50.0, Fats = 8.0, Fiber = 7.0 },
            new() { Id = 3, Calories = 480, Protein = 40.0, Carbs = 10.0, Fats = 28.0, Fiber = 4.0 },
            new() { Id = 4, Calories = 280, Protein = 20.0, Carbs = 35.0, Fats = 6.0, Fiber = 2.0 },
            new() { Id = 5, Calories = 450, Protein = 38.0, Carbs = 30.0, Fats = 18.0, Fiber = 6.0 },
            new() { Id = 6, Calories = 520, Protein = 18.0, Carbs = 65.0, Fats = 22.0, Fiber = 12.0 },
        };
        await _context.NutritionFacts.AddRangeAsync(facts);
    }

    private async Task SeedIngredientsAsync()
    {
        var ingredients = new List<Ingredient>
        {
            new() { Id = 1, Name = "Chicken Breast" },
            new() { Id = 2, Name = "Olive Oil" },
            new() { Id = 3, Name = "Mixed Herbs" },
            new() { Id = 4, Name = "Oats" },
            new() { Id = 5, Name = "Whey Protein" },
            new() { Id = 6, Name = "Banana" },
            new() { Id = 7, Name = "Salmon" },
            new() { Id = 8, Name = "Mixed Greens" },
            new() { Id = 9, Name = "Greek Yogurt" },
            new() { Id = 10, Name = "Quinoa" },
        };
        await _context.Ingredients.AddRangeAsync(ingredients);
    }

    private async Task SeedMealIngredientsAsync()
    {
        var mealIngredients = new List<MealIngredient>
        {
            new() { Id = 1, MealId = 1, IngredientId = 1, Amount = "200g" },
            new() { Id = 2, MealId = 1, IngredientId = 2, Amount = "1 tbsp" },
            new() { Id = 3, MealId = 2, IngredientId = 4, Amount = "100g" },
            new() { Id = 4, MealId = 2, IngredientId = 5, Amount = "1 scoop" },
            new() { Id = 5, MealId = 3, IngredientId = 7, Amount = "150g" },
            new() { Id = 6, MealId = 6, IngredientId = 10, Amount = "100g" },
        };
        await _context.MealIngredients.AddRangeAsync(mealIngredients);
    }

    private async Task SeedMealPlanItemsAsync()
    {
        var items = new List<MealPlanItem>
        {
            new() { Id = 1, MealPlanId = 1, MealId = 2, DayOfWeek = "Monday", MealTime = "Breakfast" },
            new() { Id = 2, MealPlanId = 1, MealId = 1, DayOfWeek = "Monday", MealTime = "Lunch" },
            new() { Id = 3, MealPlanId = 2, MealId = 4, DayOfWeek = "Monday", MealTime = "Breakfast" },
            new() { Id = 4, MealPlanId = 2, MealId = 3, DayOfWeek = "Monday", MealTime = "Lunch" },
            new() { Id = 5, MealPlanId = 3, MealId = 5, DayOfWeek = "Monday", MealTime = "Dinner" },
            new() { Id = 6, MealPlanId = 4, MealId = 6, DayOfWeek = "Monday", MealTime = "Dinner" },
        };
        await _context.MealPlanItems.AddRangeAsync(items);
    }
}
