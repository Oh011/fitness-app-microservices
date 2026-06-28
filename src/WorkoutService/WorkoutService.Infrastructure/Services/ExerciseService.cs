using Microsoft.EntityFrameworkCore;
using Shared.Results;
using WorkoutService.Application.Abstractions.Services;
using WorkoutService.Application.Features.Exercises.Dtos;
using WorkoutService.Domain.Entities;
using WorkoutService.Domain.Enums;
using WorkoutService.Infrastructure.Persistence.Context;

namespace WorkoutService.Infrastructure.Services;

internal class ExerciseService : IExerciseService
{
    private readonly ApplicationDbContext _context;

    public ExerciseService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<PaginatedResult<ExerciseDto>>> GetAllExercisesAsync(int pageNumber = 1, int pageSize = 10)
    {
        var query = _context.Exercises
            .Where(e => e.IsActive)
            .OrderBy(e => e.Name);

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dtos = items.Select(MapToDto).ToList();
        return Result<PaginatedResult<ExerciseDto>>.Success(
            new PaginatedResult<ExerciseDto>(pageNumber, pageSize, totalCount, dtos));
    }

    public async Task<Result<ExerciseDto>> GetExerciseByIdAsync(Guid id)
    {
        var exercise = await _context.Exercises.FirstOrDefaultAsync(e => e.Id == id);
        if (exercise is null)
            return Result<ExerciseDto>.NotFound("Exercise not found.");

        return Result<ExerciseDto>.Success(MapToDto(exercise));
    }

    private static ExerciseDto MapToDto(Exercise e) => new(
        e.Id, e.Name, e.Description, e.MuscleGroup,
        e.EquipmentNeeded, e.DifficultyLevel.ToString(),
        e.VideoUrl, e.ImageUrl, e.IsActive, e.CreatedAtUtc);
}
