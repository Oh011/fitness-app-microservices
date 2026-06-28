namespace WorkoutService.Application.Features.WorkoutPlans.Dtos;

public record WorkoutPlanDto(
    Guid Id,
    string Name,
    string? Description,
    string DifficultyLevel,
    int DurationWeeks,
    bool IsCustom,
    Guid? CreatedById,
    DateTime CreatedAtUtc
);
