namespace NutritionService.Domain.Entities;

public class Meal
{
    public int Id { get; set; }
    public int MealPlanId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string MealType { get; set; } = string.Empty;
    public int PrepTimeInMinutes { get; set; }
    public string Difficulty { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string IngredientsJson { get; set; } = string.Empty;
    public string InstructionsJson { get; set; } = string.Empty;
    public string? VariationsJson { get; set; }
    public string AllergensJson { get; set; } = string.Empty;
    public string TagsJson { get; set; } = string.Empty;
}
