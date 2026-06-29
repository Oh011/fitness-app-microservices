namespace NutritionService.Domain.Entities;

public class MealPlanItem
{
    public int Id { get; set; }
    public int MealPlanId { get; set; }
    public int MealId { get; set; }
    public string DayOfWeek { get; set; } = string.Empty;
    public string MealTime { get; set; } = string.Empty;
}
