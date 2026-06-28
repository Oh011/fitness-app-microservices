using Microsoft.EntityFrameworkCore;
using Shared.Results;
using WorkoutService.Application.Abstractions.Services;
using WorkoutService.Application.Features.Workouts.Dtos;
using WorkoutService.Domain.Entities;
using WorkoutService.Domain.Enums;
using WorkoutService.Infrastructure.Persistence.Context;

namespace WorkoutService.Infrastructure.Services;

internal class WorkoutService : IWorkoutService
{
    private readonly ApplicationDbContext _context;

    public WorkoutService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<PaginatedResult<WorkoutDto>>> GetAllWorkoutsAsync(int pageNumber = 1, int pageSize = 10)
    {
        var query = _context.Workouts
            .Where(w => w.IsActive)
            .OrderByDescending(w => w.CreatedAtUtc);

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dtos = items.Select(MapToDto).ToList();
        return Result<PaginatedResult<WorkoutDto>>.Success(
            new PaginatedResult<WorkoutDto>(pageNumber, pageSize, totalCount, dtos));
    }

    public async Task<Result<WorkoutDto>> GetWorkoutByIdAsync(Guid id)
    {
        var workout = await _context.Workouts.FirstOrDefaultAsync(w => w.Id == id);
        if (workout is null)
            return Result<WorkoutDto>.NotFound("Workout not found.");

        return Result<WorkoutDto>.Success(MapToDto(workout));
    }

    public async Task<Result<List<WorkoutDto>>> GetWorkoutsByPlanIdAsync(Guid planId)
    {
        var workouts = await _context.Workouts
            .Where(w => w.PlanId == planId && w.IsActive)
            .OrderBy(w => w.CreatedAtUtc)
            .ToListAsync();

        return Result<List<WorkoutDto>>.Success(workouts.Select(MapToDto).ToList());
    }

    public async Task<Result<List<WorkoutDto>>> GetWorkoutsByCategoryAsync(string categoryName)
    {
        if (!Enum.TryParse<WorkoutCategory>(categoryName, ignoreCase: true, out var category))
            return Result<List<WorkoutDto>>.Failure($"Invalid workout category: {categoryName}");

        var workouts = await _context.Workouts
            .Where(w => w.WorkoutCategory == category && w.IsActive)
            .OrderBy(w => w.CreatedAtUtc)
            .ToListAsync();

        return Result<List<WorkoutDto>>.Success(workouts.Select(MapToDto).ToList());
    }

    public async Task<Result<WorkoutSessionDto>> StartWorkoutSessionAsync(Guid workoutId, Guid userId)
    {
        var workout = await _context.Workouts.FirstOrDefaultAsync(w => w.Id == workoutId);
        if (workout is null)
            return Result<WorkoutSessionDto>.NotFound("Workout not found.");

        var session = new WorkoutSession
        {
            Id = Guid.NewGuid(),
            WorkoutId = workoutId,
            UserId = userId,
            StartedAtUtc = DateTime.UtcNow,
            Status = SessionStatus.InProgress
        };

        _context.WorkoutSessions.Add(session);
        await _context.SaveChangesAsync();

        return Result<WorkoutSessionDto>.Success(MapToSessionDto(session));
    }

    private static WorkoutDto MapToDto(Workout w) => new(
        w.Id, w.PlanId, w.Name, w.Description,
        w.WorkoutCategory.ToString(), w.DifficultyLevel.ToString(),
        w.DurationMinutes, w.CaloriesBurnedEstimate, w.IsActive, w.CreatedAtUtc);

    private static WorkoutSessionDto MapToSessionDto(WorkoutSession s) => new(
        s.Id, s.WorkoutId, s.UserId, s.StartedAtUtc,
        s.CompletedAtUtc, s.Status.ToString(), s.Notes);
}
