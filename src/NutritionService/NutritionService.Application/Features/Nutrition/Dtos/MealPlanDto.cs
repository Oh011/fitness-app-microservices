namespace NutritionService.Application.Features.Nutrition.Dtos;

public record MealPlanDto(
    int Id,
    string Name,
    string? Description,
    int CalorieTarget,
    List<MealPlanItemDto> Items
);
