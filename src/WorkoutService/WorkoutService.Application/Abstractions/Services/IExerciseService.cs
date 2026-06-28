using Shared.Results;
using WorkoutService.Application.Features.Exercises.Dtos;

namespace WorkoutService.Application.Abstractions.Services;

public interface IExerciseService
{
    Task<Result<PaginatedResult<ExerciseDto>>> GetAllExercisesAsync(int pageNumber = 1, int pageSize = 10);
    Task<Result<ExerciseDto>> GetExerciseByIdAsync(Guid id);
}
