using FCEService.Domain.Enums;

namespace FCEService.Application.DTOs;

public class FitnessStatsResponse
{
    public double Weight { get; set; }
    public double Height { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; } = string.Empty;
    public Goal Goal { get; set; }
    public ActivityLevel ActivityLevel { get; set; }
}
