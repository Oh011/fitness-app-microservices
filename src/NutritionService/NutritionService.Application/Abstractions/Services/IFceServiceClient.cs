using Shared.Results;

namespace NutritionService.Application.Abstractions.Services;

public class CalorieTargetDto
{
    public int TargetCalories { get; set; }
}

public interface IFceServiceClient
{
    Task<Result<CalorieTargetDto>> GetCalorieTargetAsync(int userId);
}
