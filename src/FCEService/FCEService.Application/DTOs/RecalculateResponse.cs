namespace FCEService.Application.DTOs;

public class RecalculateResponse
{
    public double Bmr { get; set; }
    public double Tdee { get; set; }
    public double CalorieTarget { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool PlanReassignment { get; set; }
    public AssignPlanResponse? NewPlan { get; set; }
}
