using WorkoutService.Domain.Enums;

namespace WorkoutService.Domain.Entities;

public class Workout
{
    public Guid Id { get; set; }
    public Guid PlanId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public WorkoutCategory WorkoutCategory { get; set; }
    public DifficultyLevel DifficultyLevel { get; set; }
    public int DurationMinutes { get; set; }
    public int? CaloriesBurnedEstimate { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
