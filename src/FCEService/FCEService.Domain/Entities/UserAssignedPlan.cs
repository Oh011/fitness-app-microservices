namespace FCEService.Domain.Entities;

public class UserAssignedPlan
{
    public int Id { get; set; }
    public string ExternalPlanId { get; set; } = string.Empty;
    public DateTime AssignedAt { get; set; }
    public bool IsActive { get; set; }
}
