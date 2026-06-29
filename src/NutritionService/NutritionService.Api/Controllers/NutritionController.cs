using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Responses;
using NutritionService.Api.Extensions;
using NutritionService.Application.Abstractions.Services;

namespace NutritionService.Api.Controllers;

[ApiController]
[Route("api/v1/nutrition")]
[Authorize]
public class NutritionController : ControllerBase
{
    private readonly INutritionService _nutritionService;

    public NutritionController(INutritionService nutritionService)
    {
        _nutritionService = nutritionService;
    }

    /// <summary>
    /// Gets personalized meal recommendations for the authenticated user based on their FCE CalorieTarget.
    /// </summary>
    [HttpGet("recommendations")]
    public async Task<ActionResult<ApiResponse>> GetRecommendations(
        [FromQuery] string? mealType,
        [FromQuery] int? maxCalories,
        [FromQuery] int? minProtein,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            return Unauthorized(ApiResponse.Unauthorized("User not identified."));

        var result = await _nutritionService.GetRecommendationsAsync(userId, mealType, maxCalories, minProtein, page, pageSize);
        return result.ToApiResponse();
    }

    /// <summary>
    /// Gets personalized meal recommendations for a specific user by their FCE CalorieTarget.
    /// </summary>
    [HttpGet("recommendations/{userId:int}")]
    public async Task<ActionResult<ApiResponse>> GetRecommendationsByUserId(
        int userId,
        [FromQuery] string? mealType,
        [FromQuery] int? maxCalories,
        [FromQuery] int? minProtein,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var currentUserIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(currentUserIdClaim) || !int.TryParse(currentUserIdClaim, out var currentUserId))
            return Unauthorized(ApiResponse.Unauthorized("User not identified."));

        var result = await _nutritionService.GetRecommendationsByUserIdAsync(currentUserId, userId, mealType, maxCalories, minProtein, page, pageSize);
        return result.ToApiResponse();
    }

    /// <summary>
    /// Gets detailed information about a specific meal including nutrition facts, ingredients, and instructions.
    /// </summary>
    [HttpGet("meals/{id:int}")]
    public async Task<ActionResult<ApiResponse>> GetMealDetail(int id)
    {
        var result = await _nutritionService.GetMealDetailAsync(id);
        return result.ToApiResponse();
    }

    /// <summary>
    /// Gets a paginated list of meal plans with their scheduled items.
    /// </summary>
    [HttpGet("meal-plans")]
    public async Task<ActionResult<ApiResponse>> GetMealPlans(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var result = await _nutritionService.GetMealPlansAsync(page, pageSize);
        return result.ToApiResponse();
    }

    /// <summary>
    /// Gets meal plans that match a specific calorie target value.
    /// </summary>
    [HttpGet("meal-plans/by-calories")]
    public async Task<ActionResult<ApiResponse>> GetMealPlansByCalories(
        [FromQuery] int? calories)
    {
        if (!calories.HasValue)
            return BadRequest(ApiResponse.Validation(null, "Calories query parameter is required"));

        var result = await _nutritionService.GetMealPlansByCaloriesAsync(calories.Value);
        return result.ToApiResponse();
    }
}
