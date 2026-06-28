using WorkoutService.Domain.Enums;

namespace WorkoutService.Domain.Entities;

public class WorkoutSession
{
    public Guid Id { get; set; }
    public Guid WorkoutId { get; set; }
    public Guid UserId { get; set; }
    public DateTime StartedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAtUtc { get; set; }
    public SessionStatus Status { get; set; } = SessionStatus.NotStarted;
    public string? Notes { get; set; }
}
