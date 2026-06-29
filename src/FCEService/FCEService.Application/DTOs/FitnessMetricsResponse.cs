namespace FCEService.Application.DTOs;

public class FitnessMetricsResponse
{
    public double Bmr { get; set; }
    public double Tdee { get; set; }
    public double CalorieTarget { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CalculatedAt { get; set; }
}
