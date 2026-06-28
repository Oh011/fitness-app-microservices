using WorkoutService.Domain.Enums;

namespace WorkoutService.Domain.Entities;

public class Exercise
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? MuscleGroup { get; set; }
    public string? EquipmentNeeded { get; set; }
    public DifficultyLevel DifficultyLevel { get; set; }
    public string? VideoUrl { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
