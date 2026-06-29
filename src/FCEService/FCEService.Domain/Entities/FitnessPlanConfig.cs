using FCEService.Domain.Enums;

namespace FCEService.Domain.Entities;

public class FitnessPlanConfig
{
    public int Id { get; set; }
    public Goal Goal { get; set; }
    public FitnessStatus Status { get; set; }
    public double CalorieMin { get; set; }
    public double CalorieMax { get; set; }
    public string ExternalPlanId { get; set; } = string.Empty;
    public string PlanName { get; set; } = string.Empty;
    public int WorkoutsPerWeek { get; set; }
}
