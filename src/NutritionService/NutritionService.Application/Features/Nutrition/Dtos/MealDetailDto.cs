namespace NutritionService.Application.Features.Nutrition.Dtos;

public record MealDetailDto(
    int Id,
    int MealPlanId,
    string Name,
    string MealType,
    int PrepTimeInMinutes,
    string Difficulty,
    string ImageUrl,
    int Calories,
    double Protein,
    double Carbs,
    double Fats,
    double Fiber,
    string IngredientsJson,
    string InstructionsJson,
    string? VariationsJson,
    string AllergensJson,
    string TagsJson
);
