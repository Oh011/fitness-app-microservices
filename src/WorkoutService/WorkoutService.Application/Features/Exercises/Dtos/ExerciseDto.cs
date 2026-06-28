namespace WorkoutService.Application.Features.Exercises.Dtos;

public record ExerciseDto(
    Guid Id,
    string Name,
    string? Description,
    string? MuscleGroup,
    string? EquipmentNeeded,
    string DifficultyLevel,
    string? VideoUrl,
    string? ImageUrl,
    bool IsActive,
    DateTime CreatedAtUtc
);
