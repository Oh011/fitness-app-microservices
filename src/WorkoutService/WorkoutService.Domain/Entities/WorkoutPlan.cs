using WorkoutService.Domain.Enums;

namespace WorkoutService.Domain.Entities;

public class WorkoutPlan
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DifficultyLevel DifficultyLevel { get; set; }
    public int DurationWeeks { get; set; }
    public bool IsCustom { get; set; }
    public Guid? CreatedById { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
