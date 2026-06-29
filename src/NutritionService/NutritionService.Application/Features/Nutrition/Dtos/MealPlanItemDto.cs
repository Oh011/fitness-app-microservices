namespace NutritionService.Application.Features.Nutrition.Dtos;

public record MealPlanItemDto(
    int Id,
    int MealPlanId,
    int MealId,
    string MealName,
    string DayOfWeek,
    string MealTime
);
