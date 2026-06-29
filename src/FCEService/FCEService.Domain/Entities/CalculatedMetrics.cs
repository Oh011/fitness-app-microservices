using FCEService.Domain.Enums;

namespace FCEService.Domain.Entities;

public class CalculatedMetrics
{
    public int Id { get; set; }
    public double BMR { get; set; }
    public double TDEE { get; set; }
    public double CalorieTarget { get; set; }
    public FitnessStatus Status { get; set; }
    public DateTime CalculatedAt { get; set; }
}
