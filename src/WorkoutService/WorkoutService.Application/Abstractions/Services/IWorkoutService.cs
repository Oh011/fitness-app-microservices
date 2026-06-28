using Shared.Results;
using WorkoutService.Application.Features.Workouts.Dtos;

namespace WorkoutService.Application.Abstractions.Services;

public interface IWorkoutService
{
    Task<Result<PaginatedResult<WorkoutDto>>> GetAllWorkoutsAsync(int pageNumber = 1, int pageSize = 10);
    Task<Result<WorkoutDto>> GetWorkoutByIdAsync(Guid id);
    Task<Result<List<WorkoutDto>>> GetWorkoutsByPlanIdAsync(Guid planId);
    Task<Result<List<WorkoutDto>>> GetWorkoutsByCategoryAsync(string categoryName);
    Task<Result<WorkoutSessionDto>> StartWorkoutSessionAsync(Guid workoutId, Guid userId);
}
