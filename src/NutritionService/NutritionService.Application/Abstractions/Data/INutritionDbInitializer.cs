namespace NutritionService.Application.Abstractions.Data;

public interface INutritionDbInitializer
{
    Task InitializeAsync();
}
