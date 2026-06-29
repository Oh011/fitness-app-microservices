namespace NutritionService.Application.Features.Nutrition.Dtos;

public record RecommendationResponseDto(
    int UserDailyGoalCalories,
    List<MealDto> RecommendedMeals
);
