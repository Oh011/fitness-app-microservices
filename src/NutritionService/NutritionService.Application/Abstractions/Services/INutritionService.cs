using Shared.Results;
using NutritionService.Application.Features.Nutrition.Dtos;

namespace NutritionService.Application.Abstractions.Services;

public interface INutritionService
{
    Task<Result<RecommendationResponseDto>> GetRecommendationsAsync(
        int userId, string? mealType, int? maxCalories, int? minProtein, int page = 1, int pageSize = 20);

    Task<Result<RecommendationResponseDto>> GetRecommendationsByUserIdAsync(
        int userId, int targetUserId, string? mealType, int? maxCalories, int? minProtein, int page = 1, int pageSize = 20);

    Task<Result<MealDetailDto>> GetMealDetailAsync(int id);

    Task<Result<PaginatedResult<MealPlanDto>>> GetMealPlansAsync(int page = 1, int pageSize = 20);

    Task<Result<List<MealPlanDto>>> GetMealPlansByCaloriesAsync(int calories);
}
