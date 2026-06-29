namespace NutritionService.Application.Features.Nutrition.Dtos;

public record MealDto(
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
    string TagsJson
);
