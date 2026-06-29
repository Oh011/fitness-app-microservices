using FCEService.Domain.Entities;
using FCEService.Domain.Enums;
using FCEService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FCEService.Infrastructure.DataSeeding;

internal class FCEDbInitializer : IFCEDbInitializer
{
    private readonly FCEDbContext _context;
    private readonly ILogger<FCEDbInitializer> _logger;

    public FCEDbInitializer(FCEDbContext context, ILogger<FCEDbInitializer> logger)
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

            if (!await _context.FitnessPlanConfigs.AnyAsync())
                await SeedPlanConfigsAsync();

            await _context.SaveChangesAsync();
            _logger.LogInformation("FCE database seeded successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the FCE database.");
            throw;
        }
    }

    private async Task SeedPlanConfigsAsync()
    {
        var plans = new List<FitnessPlanConfig>
        {
            // Lose Weight plans
            new() { Goal = Goal.LoseWeight, Status = FitnessStatus.Weak, CalorieMin = 0, CalorieMax = 1800, ExternalPlanId = "plan_lw_weak", PlanName = "Weight Loss - Starter", WorkoutsPerWeek = 3 },
            new() { Goal = Goal.LoseWeight, Status = FitnessStatus.Normal, CalorieMin = 1801, CalorieMax = 2500, ExternalPlanId = "plan_lw_normal", PlanName = "Weight Loss - Normal Intensity", WorkoutsPerWeek = 4 },
            new() { Goal = Goal.LoseWeight, Status = FitnessStatus.Hard, CalorieMin = 2501, CalorieMax = 9999, ExternalPlanId = "plan_lw_hard", PlanName = "Weight Loss - Hard Intensity", WorkoutsPerWeek = 5 },

            // Get Fitter plans
            new() { Goal = Goal.GetFitter, Status = FitnessStatus.Weak, CalorieMin = 0, CalorieMax = 1800, ExternalPlanId = "plan_gf_weak", PlanName = "Get Fitter - Starter", WorkoutsPerWeek = 3 },
            new() { Goal = Goal.GetFitter, Status = FitnessStatus.Normal, CalorieMin = 1801, CalorieMax = 2500, ExternalPlanId = "plan_gf_normal", PlanName = "Get Fitter - Moderate", WorkoutsPerWeek = 4 },
            new() { Goal = Goal.GetFitter, Status = FitnessStatus.Hard, CalorieMin = 2501, CalorieMax = 9999, ExternalPlanId = "plan_gf_hard", PlanName = "Get Fitter - Intense", WorkoutsPerWeek = 5 },

            // Gain Weight plans
            new() { Goal = Goal.GainWeight, Status = FitnessStatus.Weak, CalorieMin = 0, CalorieMax = 1800, ExternalPlanId = "plan_gw_weak", PlanName = "Weight Gain - Starter", WorkoutsPerWeek = 3 },
            new() { Goal = Goal.GainWeight, Status = FitnessStatus.Normal, CalorieMin = 1801, CalorieMax = 2500, ExternalPlanId = "plan_gw_normal", PlanName = "Weight Gain - Moderate", WorkoutsPerWeek = 4 },
            new() { Goal = Goal.GainWeight, Status = FitnessStatus.Hard, CalorieMin = 2501, CalorieMax = 9999, ExternalPlanId = "plan_gw_hard", PlanName = "Weight Gain - Intense", WorkoutsPerWeek = 5 },

            // Gain More Flexible plans
            new() { Goal = Goal.GainMoreFlexible, Status = FitnessStatus.Weak, CalorieMin = 0, CalorieMax = 1800, ExternalPlanId = "plan_gmf_weak", PlanName = "Flexibility - Starter", WorkoutsPerWeek = 3 },
            new() { Goal = Goal.GainMoreFlexible, Status = FitnessStatus.Normal, CalorieMin = 1801, CalorieMax = 2500, ExternalPlanId = "plan_gmf_normal", PlanName = "Flexibility - Moderate", WorkoutsPerWeek = 4 },
            new() { Goal = Goal.GainMoreFlexible, Status = FitnessStatus.Hard, CalorieMin = 2501, CalorieMax = 9999, ExternalPlanId = "plan_gmf_hard", PlanName = "Flexibility - Intense", WorkoutsPerWeek = 5 },

            // Learn the Basic plans
            new() { Goal = Goal.LearnTheBasic, Status = FitnessStatus.Weak, CalorieMin = 0, CalorieMax = 1800, ExternalPlanId = "plan_ltb_weak", PlanName = "Basics - Starter", WorkoutsPerWeek = 2 },
            new() { Goal = Goal.LearnTheBasic, Status = FitnessStatus.Normal, CalorieMin = 1801, CalorieMax = 2500, ExternalPlanId = "plan_ltb_normal", PlanName = "Basics - Moderate", WorkoutsPerWeek = 3 },
            new() { Goal = Goal.LearnTheBasic, Status = FitnessStatus.Hard, CalorieMin = 2501, CalorieMax = 9999, ExternalPlanId = "plan_ltb_hard", PlanName = "Basics - Intense", WorkoutsPerWeek = 4 },
        };

        await _context.FitnessPlanConfigs.AddRangeAsync(plans);
    }
}
