using Microsoft.EntityFrameworkCore;
using Shared.Results;
using WorkoutService.Application.Abstractions.Services;
using WorkoutService.Application.Features.WorkoutPlans.Dtos;
using WorkoutService.Domain.Entities;
using WorkoutService.Domain.Enums;
using WorkoutService.Infrastructure.Persistence.Context;

namespace WorkoutService.Infrastructure.Services;

internal class WorkoutPlanService : IWorkoutPlanService
{
    private readonly ApplicationDbContext _context;

    public WorkoutPlanService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<PaginatedResult<WorkoutPlanDto>>> GetAllWorkoutPlansAsync(int pageNumber = 1, int pageSize = 10)
    {
        var query = _context.WorkoutPlans
            .OrderByDescending(wp => wp.CreatedAtUtc);

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dtos = items.Select(MapToDto).ToList();
        return Result<PaginatedResult<WorkoutPlanDto>>.Success(
            new PaginatedResult<WorkoutPlanDto>(pageNumber, pageSize, totalCount, dtos));
    }

    public async Task<Result<WorkoutPlanDto>> GetWorkoutPlanByIdAsync(Guid id)
    {
        var plan = await _context.WorkoutPlans.FirstOrDefaultAsync(wp => wp.Id == id);
        if (plan is null)
            return Result<WorkoutPlanDto>.NotFound("Workout plan not found.");

        return Result<WorkoutPlanDto>.Success(MapToDto(plan));
    }

    private static WorkoutPlanDto MapToDto(WorkoutPlan wp) => new(
        wp.Id, wp.Name, wp.Description, wp.DifficultyLevel.ToString(),
        wp.DurationWeeks, wp.IsCustom, wp.CreatedById, wp.CreatedAtUtc);
}
