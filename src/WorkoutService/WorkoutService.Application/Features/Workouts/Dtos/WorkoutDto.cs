namespace WorkoutService.Application.Features.Workouts.Dtos;

public record WorkoutDto(
    Guid Id,
    Guid PlanId,
    string Name,
    string? Description,
    string WorkoutCategory,
    string DifficultyLevel,
    int DurationMinutes,
    int? CaloriesBurnedEstimate,
    bool IsActive,
    DateTime CreatedAtUtc
);
