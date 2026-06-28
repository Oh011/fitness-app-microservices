using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Responses;
using WorkoutService.Api.Extensions;
using WorkoutService.Application.Abstractions.Services;

namespace WorkoutService.Api.Controllers;

[ApiController]
[Route("api/workout-plans")]
[Authorize]
public class WorkoutPlansController : ControllerBase
{
    private readonly IWorkoutPlanService _workoutPlanService;

    public WorkoutPlansController(IWorkoutPlanService workoutPlanService)
    {
        _workoutPlanService = workoutPlanService;
    }

    /// <summary>
    /// Gets all workout plans with pagination.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetAllWorkoutPlans([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _workoutPlanService.GetAllWorkoutPlansAsync(pageNumber, pageSize);
        return result.ToApiResponse();
    }

    /// <summary>
    /// Gets a specific workout plan by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse>> GetWorkoutPlanById(Guid id)
    {
        var result = await _workoutPlanService.GetWorkoutPlanByIdAsync(id);
        return result.ToApiResponse();
    }
}
