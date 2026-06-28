using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Responses;
using WorkoutService.Api.Extensions;
using WorkoutService.Application.Abstractions.Services;

namespace WorkoutService.Api.Controllers;

[ApiController]
[Route("api/workouts")]
[Authorize]
public class WorkoutsController : ControllerBase
{
    private readonly IWorkoutService _workoutService;

    public WorkoutsController(IWorkoutService workoutService)
    {
        _workoutService = workoutService;
    }

    /// <summary>
    /// Gets all active workouts with pagination.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetAllWorkouts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _workoutService.GetAllWorkoutsAsync(pageNumber, pageSize);
        return result.ToApiResponse();
    }

    /// <summary>
    /// Gets a specific workout by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse>> GetWorkoutById(Guid id)
    {
        var result = await _workoutService.GetWorkoutByIdAsync(id);
        return result.ToApiResponse();
    }

    /// <summary>
    /// Gets all workouts belonging to a specific plan.
    /// </summary>
    [HttpGet("by-plan/{planId:guid}")]
    public async Task<ActionResult<ApiResponse>> GetWorkoutsByPlanId(Guid planId)
    {
        var result = await _workoutService.GetWorkoutsByPlanIdAsync(planId);
        return result.ToApiResponse();
    }

    /// <summary>
    /// Gets all workouts in a specific category (e.g., Strength, Cardio, Yoga).
    /// </summary>
    [HttpGet("category/{categoryName}")]
    public async Task<ActionResult<ApiResponse>> GetWorkoutsByCategory(string categoryName)
    {
        var result = await _workoutService.GetWorkoutsByCategoryAsync(categoryName);
        return result.ToApiResponse();
    }

    /// <summary>
    /// Starts a workout session for the authenticated user.
    /// </summary>
    [HttpPost("{id:guid}/start")]
    public async Task<ActionResult<ApiResponse>> StartWorkout(Guid id)
    {
        // Get current user ID from JWT claims
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(ApiResponse.Unauthorized("User not identified."));

        var result = await _workoutService.StartWorkoutSessionAsync(id, userId);
        return result.ToApiResponse();
    }
}
