using Shared.Results;

namespace NutritionService.Application.Abstractions.Services;

public class CalorieTargetDto
{
    public int TargetCalories { get; set; }
    public string Goal { get; set; } = string.Empty;
    public string ActivityLevel { get; set; } = string.Empty;
}

public interface IFceServiceClient
{
    Task<Result<CalorieTargetDto>> GetCalorieTargetAsync(int userId);
}
