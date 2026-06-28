using Shared.Results;
using WorkoutService.Application.Features.WorkoutPlans.Dtos;

namespace WorkoutService.Application.Abstractions.Services;

public interface IWorkoutPlanService
{
    Task<Result<PaginatedResult<WorkoutPlanDto>>> GetAllWorkoutPlansAsync(int pageNumber = 1, int pageSize = 10);
    Task<Result<WorkoutPlanDto>> GetWorkoutPlanByIdAsync(Guid id);
}
