namespace NutritionService.Domain.Entities;

public class MealIngredient
{
    public int Id { get; set; }
    public int MealId { get; set; }
    public int IngredientId { get; set; }
    public string Amount { get; set; } = string.Empty;
}
