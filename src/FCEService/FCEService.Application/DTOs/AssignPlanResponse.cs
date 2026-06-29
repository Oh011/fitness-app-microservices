namespace FCEService.Application.DTOs;

public class AssignPlanResponse
{
    public int PlanId { get; set; }
    public string ExternalPlanId { get; set; } = string.Empty;
    public string PlanName { get; set; } = string.Empty;
    public int WorkoutsPerWeek { get; set; }
    public double CalorieMin { get; set; }
    public double CalorieMax { get; set; }
}
