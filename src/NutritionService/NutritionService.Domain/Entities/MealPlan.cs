namespace NutritionService.Domain.Entities;

public class MealPlan
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CalorieTarget { get; set; }
}
