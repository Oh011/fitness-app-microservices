using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkoutService.Application.Abstractions.Data;
using WorkoutService.Domain.Entities;
using WorkoutService.Domain.Enums;
using WorkoutService.Infrastructure.Persistence.Context;

namespace WorkoutService.Infrastructure.DataSeeding;

internal class WorkoutDbInitializer : IWorkoutDbInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<WorkoutDbInitializer> _logger;

    public WorkoutDbInitializer(ApplicationDbContext context, ILogger<WorkoutDbInitializer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        try
        {
            if (_context.Database.GetPendingMigrations().Any())
                await _context.Database.MigrateAsync();

            if (!await _context.WorkoutPlans.AnyAsync())
                await SeedWorkoutPlansAsync();

            if (!await _context.Exercises.AnyAsync())
                await SeedExercisesAsync();

            if (!await _context.Workouts.AnyAsync())
                await SeedWorkoutsAsync();

            await _context.SaveChangesAsync();
            _logger.LogInformation("Workout database seeded successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the workout database.");
            throw;
        }
    }

    private async Task SeedWorkoutPlansAsync()
    {
        var plans = new List<WorkoutPlan>
        {
            new()
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000001"),
                Name = "Beginner Strength",
                Description = "A 4-week program to build foundational strength with basic compound movements.",
                DifficultyLevel = DifficultyLevel.Beginner,
                DurationWeeks = 4,
                IsCustom = false,
                CreatedAtUtc = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000002"),
                Name = "Fat Burn Express",
                Description = "High-intensity cardio and HIIT workouts to maximize calorie burn in 30 minutes.",
                DifficultyLevel = DifficultyLevel.Intermediate,
                DurationWeeks = 6,
                IsCustom = false,
                CreatedAtUtc = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000003"),
                Name = "Advanced Powerlifting",
                Description = "An 8-week periodized program for experienced lifters focused on the big three lifts.",
                DifficultyLevel = DifficultyLevel.Advanced,
                DurationWeeks = 8,
                IsCustom = false,
                CreatedAtUtc = DateTime.UtcNow
            }
        };

        await _context.WorkoutPlans.AddRangeAsync(plans);
    }

    private async Task SeedExercisesAsync()
    {
        var exercises = new List<Exercise>
        {
            new()
            {
                Id = Guid.Parse("20000000-0000-0000-0000-000000000001"),
                Name = "Bench Press",
                Description = "Lie on a flat bench and press a barbell upward from chest level.",
                MuscleGroup = "Chest",
                EquipmentNeeded = "Barbell, Bench",
                DifficultyLevel = DifficultyLevel.Beginner,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.Parse("20000000-0000-0000-0000-000000000002"),
                Name = "Squat",
                Description = "Stand with barbell on upper back, bend knees to lower body, then stand back up.",
                MuscleGroup = "Legs",
                EquipmentNeeded = "Barbell, Squat Rack",
                DifficultyLevel = DifficultyLevel.Intermediate,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.Parse("20000000-0000-0000-0000-000000000003"),
                Name = "Deadlift",
                Description = "Lift a loaded barbell from the ground to hip level with a straight back.",
                MuscleGroup = "Back",
                EquipmentNeeded = "Barbell",
                DifficultyLevel = DifficultyLevel.Intermediate,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.Parse("20000000-0000-0000-0000-000000000004"),
                Name = "Pull-Up",
                Description = "Hang from a bar and pull your body up until chin clears the bar.",
                MuscleGroup = "Back",
                EquipmentNeeded = "Pull-Up Bar",
                DifficultyLevel = DifficultyLevel.Intermediate,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.Parse("20000000-0000-0000-0000-000000000005"),
                Name = "Overhead Press",
                Description = "Press a barbell from shoulder level to overhead.",
                MuscleGroup = "Shoulders",
                EquipmentNeeded = "Barbell",
                DifficultyLevel = DifficultyLevel.Beginner,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.Parse("20000000-0000-0000-0000-000000000006"),
                Name = "Running (Treadmill)",
                Description = "Run on a treadmill at moderate to high intensity.",
                MuscleGroup = "Cardio",
                EquipmentNeeded = "Treadmill",
                DifficultyLevel = DifficultyLevel.Beginner,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.Parse("20000000-0000-0000-0000-000000000007"),
                Name = "Burpee",
                Description = "From standing, drop to plank, do a push-up, jump feet back in, and leap up.",
                MuscleGroup = "Full Body",
                EquipmentNeeded = "None",
                DifficultyLevel = DifficultyLevel.Intermediate,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.Parse("20000000-0000-0000-0000-000000000008"),
                Name = "Dumbbell Row",
                Description = "Hinge at hips with one arm on a bench, row a dumbbell to your side.",
                MuscleGroup = "Back",
                EquipmentNeeded = "Dumbbell, Bench",
                DifficultyLevel = DifficultyLevel.Beginner,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow
            }
        };

        await _context.Exercises.AddRangeAsync(exercises);
    }

    private async Task SeedWorkoutsAsync()
    {
        var plan1Id = Guid.Parse("10000000-0000-0000-0000-000000000001");
        var plan2Id = Guid.Parse("10000000-0000-0000-0000-000000000002");
        var plan3Id = Guid.Parse("10000000-0000-0000-0000-000000000003");

        var workouts = new List<Workout>
        {
            new()
            {
                Id = Guid.Parse("30000000-0000-0000-0000-000000000001"),
                PlanId = plan1Id,
                Name = "Full Body A",
                Description = "A full body workout covering all major muscle groups.",
                WorkoutCategory = WorkoutCategory.Strength,
                DifficultyLevel = DifficultyLevel.Beginner,
                DurationMinutes = 45,
                CaloriesBurnedEstimate = 300,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.Parse("30000000-0000-0000-0000-000000000002"),
                PlanId = plan1Id,
                Name = "Full Body B",
                Description = "A second full body workout with different exercises.",
                WorkoutCategory = WorkoutCategory.Strength,
                DifficultyLevel = DifficultyLevel.Beginner,
                DurationMinutes = 45,
                CaloriesBurnedEstimate = 320,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.Parse("30000000-0000-0000-0000-000000000003"),
                PlanId = plan2Id,
                Name = "HIIT Cardio Blast",
                Description = "30 minutes of high-intensity interval training.",
                WorkoutCategory = WorkoutCategory.Hiit,
                DifficultyLevel = DifficultyLevel.Intermediate,
                DurationMinutes = 30,
                CaloriesBurnedEstimate = 400,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.Parse("30000000-0000-0000-0000-000000000004"),
                PlanId = plan3Id,
                Name = "Heavy Squat Day",
                Description = "Intensive squat-focused session with accessory work.",
                WorkoutCategory = WorkoutCategory.Strength,
                DifficultyLevel = DifficultyLevel.Advanced,
                DurationMinutes = 90,
                CaloriesBurnedEstimate = 500,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow
            }
        };

        await _context.Workouts.AddRangeAsync(workouts);
    }
}
