namespace WorkoutService.Application.Abstractions.Data;

public interface IWorkoutDbInitializer
{
    Task InitializeAsync();
}
