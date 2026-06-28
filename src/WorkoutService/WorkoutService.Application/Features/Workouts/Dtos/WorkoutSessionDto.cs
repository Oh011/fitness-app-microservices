namespace WorkoutService.Application.Features.Workouts.Dtos;

public record WorkoutSessionDto(
    Guid Id,
    Guid WorkoutId,
    Guid UserId,
    DateTime StartedAtUtc,
    DateTime? CompletedAtUtc,
    string Status,
    string? Notes
);
