using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Responses;
using WorkoutService.Api.Extensions;
using WorkoutService.Application.Abstractions.Services;

namespace WorkoutService.Api.Controllers;

[ApiController]
[Route("api/exercises")]
[Authorize]
public class ExercisesController : ControllerBase
{
    private readonly IExerciseService _exerciseService;

    public ExercisesController(IExerciseService exerciseService)
    {
        _exerciseService = exerciseService;
    }

    /// <summary>
    /// Gets all exercises with pagination.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetAllExercises([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _exerciseService.GetAllExercisesAsync(pageNumber, pageSize);
        return result.ToApiResponse();
    }

    /// <summary>
    /// Gets a specific exercise by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse>> GetExerciseById(Guid id)
    {
        var result = await _exerciseService.GetExerciseByIdAsync(id);
        return result.ToApiResponse();
    }
}
