using FCEService.Application.Abstractions;
using FCEService.Application.DTOs;
using FCEService.Domain.Entities;
using FCEService.Domain.Enums;
using FCEService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Shared.Results;

namespace FCEService.Infrastructure.Services;

internal class FitnessService : IFitnessService
{
    private readonly FCEDbContext _context;

    public FitnessService(FCEDbContext context)
    {
        _context = context;
    }

    // ============================================================
    // 3.1: Submit Weight, Goal & Activity Stats
    // ============================================================
    public async Task<Result> SubmitWeightGoalActivityAsync(WeightGoalActivityRequest request)
    {
        // Validate age
        if (request.Age < 16 || request.Age > 100)
            return Result.Validation("VAL_INVALID_AGE");

        // Validate weight
        if (request.Weight < 40 || request.Weight > 200)
            return Result.Validation("VAL_INVALID_WEIGHT");

        // Validate height
        if (request.Height < 140 || request.Height > 220)
            return Result.Validation("VAL_INVALID_HEIGHT");

        // Validate gender
        if (request.Gender != "Male" && request.Gender != "Female")
            return Result.Validation("VAL_INVALID_GENDER");

        // Validate goal
        if (!Enum.IsDefined(typeof(Goal), request.Goal))
            return Result.Validation("VAL_INVALID_GOAL");

        // Validate activity level
        if (!Enum.IsDefined(typeof(ActivityLevel), request.ActivityLevel))
            return Result.Validation("VAL_INVALID_ACTIVITY");

        var stats = new UserFitnessStats
        {
            Id = request.UserId,
            Weight = request.Weight,
            Height = request.Height,
            Age = request.Age,
            Gender = request.Gender,
            Goal = request.Goal,
            ActivityLevel = request.ActivityLevel,
            RecordedAt = DateTime.UtcNow
        };

        _context.UserFitnessStats.Add(stats);
        await _context.SaveChangesAsync();

        return Result.Created("Fitness stats recorded successfully.");
    }

    // ============================================================
    // 3.2: Calculate BMR, TDEE & Calorie Target
    // ============================================================
    public async Task<Result<CalculateResponse>> CalculateAsync(CalculateRequest request)
    {
        var stats = await _context.UserFitnessStats
            .Where(u => u.Id == request.UserId)
            .OrderByDescending(u => u.RecordedAt)
            .FirstOrDefaultAsync();

        if (stats is null)
            return Result<CalculateResponse>.NotFound("FCE_STATS_NOT_FOUND");

        double bmr = CalculateBmr(stats.Weight, stats.Height, stats.Age, stats.Gender);
        double tdee = CalculateTdee(bmr, stats.ActivityLevel);
        double calorieTarget = CalculateCalorieTarget(tdee, stats.Goal);
        var status = ClassifyStatus(calorieTarget);

        if (double.IsNaN(bmr) || double.IsInfinity(bmr) ||
            double.IsNaN(tdee) || double.IsInfinity(tdee) ||
            double.IsNaN(calorieTarget) || double.IsInfinity(calorieTarget))
        {
            return Result<CalculateResponse>.Validation("FCE_INVALID_CALCULATION");
        }

        var metrics = new CalculatedMetrics
        {
            Id = request.UserId,
            BMR = Math.Round(bmr, 2),
            TDEE = Math.Round(tdee, 2),
            CalorieTarget = Math.Round(calorieTarget, 2),
            Status = status,
            CalculatedAt = DateTime.UtcNow
        };

        // Upsert: remove existing row for this user, then add new
        var existing = await _context.CalculatedMetrics.FindAsync(request.UserId);
        if (existing is not null)
            _context.CalculatedMetrics.Remove(existing);

        _context.CalculatedMetrics.Add(metrics);
        await _context.SaveChangesAsync();

        return Result<CalculateResponse>.Success(new CalculateResponse
        {
            Bmr = metrics.BMR,
            Tdee = metrics.TDEE,
            CalorieTarget = metrics.CalorieTarget,
            Status = metrics.Status.ToString()
        });
    }

    // ============================================================
    // 3.3: Assign Fitness Plan
    // ============================================================
    public async Task<Result<AssignPlanResponse>> AssignPlanAsync(CalculateRequest request)
    {
        var metrics = await _context.CalculatedMetrics.FindAsync(request.UserId);
        if (metrics is null)
            return Result<AssignPlanResponse>.Validation("FCE_METRICS_NOT_CALCULATED");

        // Get the stats to know the user's goal
        var stats = await _context.UserFitnessStats
            .Where(u => u.Id == request.UserId)
            .OrderByDescending(u => u.RecordedAt)
            .FirstOrDefaultAsync();

        if (stats is null)
            return Result<AssignPlanResponse>.NotFound("FCE_STATS_NOT_FOUND");

        var planConfig = await _context.FitnessPlanConfigs
            .Where(p => p.Goal == stats.Goal && p.Status == metrics.Status)
            .FirstOrDefaultAsync();

        if (planConfig is null)
            return Result<AssignPlanResponse>.NotFound("FCE_NO_MATCHING_PLAN");

        // Deactivate prior active plans
        var activePlans = await _context.UserAssignedPlans
            .Where(p => p.Id == request.UserId && p.IsActive)
            .ToListAsync();

        foreach (var plan in activePlans)
        {
            plan.IsActive = false;
        }

        // Insert new assignment
        var newAssignment = new UserAssignedPlan
        {
            Id = request.UserId,
            ExternalPlanId = planConfig.ExternalPlanId,
            AssignedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.UserAssignedPlans.Add(newAssignment);

        // Append snapshot to history
        var history = new UserPlanHistory
        {
            UserId = request.UserId,
            ExternalPlanId = planConfig.ExternalPlanId,
            AssignedAt = DateTime.UtcNow,
            EndedAt = null,
            ReasonForChange = "initial_assignment"
        };

        _context.UserPlanHistory.Add(history);
        await _context.SaveChangesAsync();

        return Result<AssignPlanResponse>.Success(new AssignPlanResponse
        {
            PlanId = planConfig.Id,
            ExternalPlanId = planConfig.ExternalPlanId,
            PlanName = planConfig.PlanName,
            WorkoutsPerWeek = planConfig.WorkoutsPerWeek,
            CalorieMin = planConfig.CalorieMin,
            CalorieMax = planConfig.CalorieMax
        });
    }

    // ============================================================
    // 3.4: Get Fitness Metrics
    // ============================================================
    public async Task<Result<FitnessMetricsResponse>> GetMetricsAsync(int userId)
    {
        var metrics = await _context.CalculatedMetrics.FindAsync(userId);
        if (metrics is null)
            return Result<FitnessMetricsResponse>.Validation("FCE_METRICS_NOT_CALCULATED");

        return Result<FitnessMetricsResponse>.Success(new FitnessMetricsResponse
        {
            Bmr = metrics.BMR,
            Tdee = metrics.TDEE,
            CalorieTarget = metrics.CalorieTarget,
            Status = metrics.Status.ToString(),
            CalculatedAt = metrics.CalculatedAt
        });
    }

    // ============================================================
    // 3.5: Get Fitness Stats
    // ============================================================
    public async Task<Result<FitnessStatsResponse>> GetStatsAsync(int userId)
    {
        var stats = await _context.UserFitnessStats
            .Where(u => u.Id == userId)
            .OrderByDescending(u => u.RecordedAt)
            .FirstOrDefaultAsync();

        if (stats is null)
            return Result<FitnessStatsResponse>.NotFound("FCE_STATS_NOT_FOUND");

        return Result<FitnessStatsResponse>.Success(new FitnessStatsResponse
        {
            Weight = stats.Weight,
            Height = stats.Height,
            Age = stats.Age,
            Gender = stats.Gender,
            Goal = stats.Goal,
            ActivityLevel = stats.ActivityLevel
        });
    }

    // ============================================================
    // 3.6: Recalculate Metrics & Reassign Plan
    // ============================================================
    public async Task<Result<RecalculateResponse>> RecalculateAsync(int userId, RecalculateRequest? request)
    {
        // Find existing metrics
        var existingMetrics = await _context.CalculatedMetrics.FindAsync(userId);
        if (existingMetrics is null)
            return Result<RecalculateResponse>.Validation("FCE_METRICS_NOT_CALCULATED");

        // Get latest stats
        var stats = await _context.UserFitnessStats
            .Where(u => u.Id == userId)
            .OrderByDescending(u => u.RecordedAt)
            .FirstOrDefaultAsync();

        if (stats is null)
            return Result<RecalculateResponse>.NotFound("FCE_STATS_NOT_FOUND");

        // Use new weight if provided, otherwise use existing weight from stats
        double effectiveWeight = request?.NewWeight ?? stats.Weight;

        double bmr = CalculateBmr(effectiveWeight, stats.Height, stats.Age, stats.Gender);
        double tdee = CalculateTdee(bmr, stats.ActivityLevel);
        double calorieTarget = CalculateCalorieTarget(tdee, stats.Goal);
        var newStatus = ClassifyStatus(calorieTarget);

        if (double.IsNaN(bmr) || double.IsInfinity(bmr) ||
            double.IsNaN(tdee) || double.IsInfinity(tdee) ||
            double.IsNaN(calorieTarget) || double.IsInfinity(calorieTarget))
        {
            return Result<RecalculateResponse>.Validation("FCE_INVALID_CALCULATION");
        }

        // Update metrics in place
        existingMetrics.BMR = Math.Round(bmr, 2);
        existingMetrics.TDEE = Math.Round(tdee, 2);
        existingMetrics.CalorieTarget = Math.Round(calorieTarget, 2);
        existingMetrics.Status = newStatus;
        existingMetrics.CalculatedAt = DateTime.UtcNow;

        bool planReassignment = newStatus != existingMetrics.Status;
        AssignPlanResponse? newPlan = null;

        if (planReassignment)
        {
            // Find matching plan config
            var planConfig = await _context.FitnessPlanConfigs
                .Where(p => p.Goal == stats.Goal && p.Status == newStatus)
                .FirstOrDefaultAsync();

            if (planConfig is not null)
            {
                // Deactivate current active plans
                var activePlans = await _context.UserAssignedPlans
                    .Where(p => p.Id == userId && p.IsActive)
                    .ToListAsync();

                foreach (var plan in activePlans)
                {
                    plan.IsActive = false;
                }

                // Insert new assignment
                var newAssignment = new UserAssignedPlan
                {
                    Id = userId,
                    ExternalPlanId = planConfig.ExternalPlanId,
                    AssignedAt = DateTime.UtcNow,
                    IsActive = true
                };

                _context.UserAssignedPlans.Add(newAssignment);

                // Append to history
                var history = new UserPlanHistory
                {
                    UserId = userId,
                    ExternalPlanId = planConfig.ExternalPlanId,
                    AssignedAt = DateTime.UtcNow,
                    EndedAt = null,
                    ReasonForChange = request?.Reason ?? "weight_update"
                };

                _context.UserPlanHistory.Add(history);

                newPlan = new AssignPlanResponse
                {
                    PlanId = planConfig.Id,
                    ExternalPlanId = planConfig.ExternalPlanId,
                    PlanName = planConfig.PlanName,
                    WorkoutsPerWeek = planConfig.WorkoutsPerWeek,
                    CalorieMin = planConfig.CalorieMin,
                    CalorieMax = planConfig.CalorieMax
                };
            }
        }

        await _context.SaveChangesAsync();

        return Result<RecalculateResponse>.Success(new RecalculateResponse
        {
            Bmr = existingMetrics.BMR,
            Tdee = existingMetrics.TDEE,
            CalorieTarget = existingMetrics.CalorieTarget,
            Status = existingMetrics.Status.ToString(),
            PlanReassignment = planReassignment,
            NewPlan = newPlan
        });
    }

    // ============================================================
    // 3.7: Get Fitness Plan Configurations (Paginated)
    // ============================================================
    public async Task<Result<PlanConfigListResponse>> GetPlanConfigsAsync(
        Goal? goal, FitnessStatus? status, int page = 1, int pageSize = 20)
    {
        var query = _context.FitnessPlanConfigs.AsQueryable();

        if (goal.HasValue)
            query = query.Where(p => p.Goal == goal.Value);

        if (status.HasValue)
            query = query.Where(p => p.Status == status.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(p => p.Goal)
            .ThenBy(p => p.Status)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new PlanConfigDto
            {
                Id = p.Id,
                Goal = p.Goal,
                Status = p.Status,
                CalorieMin = p.CalorieMin,
                CalorieMax = p.CalorieMax,
                ExternalPlanId = p.ExternalPlanId,
                PlanName = p.PlanName,
                WorkoutsPerWeek = p.WorkoutsPerWeek
            })
            .ToListAsync();

        return Result<PlanConfigListResponse>.Success(new PlanConfigListResponse
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = items
        });
    }

    // ============================================================
    // 3.8: Get Specific Plan Configuration
    // ============================================================
    public async Task<Result<PlanConfigDto>> GetPlanByIdAsync(int planId)
    {
        var plan = await _context.FitnessPlanConfigs.FindAsync(planId);
        if (plan is null)
            return Result<PlanConfigDto>.NotFound("RES_PLAN_NOT_FOUND");

        return Result<PlanConfigDto>.Success(new PlanConfigDto
        {
            Id = plan.Id,
            Goal = plan.Goal,
            Status = plan.Status,
            CalorieMin = plan.CalorieMin,
            CalorieMax = plan.CalorieMax,
            ExternalPlanId = plan.ExternalPlanId,
            PlanName = plan.PlanName,
            WorkoutsPerWeek = plan.WorkoutsPerWeek
        });
    }

    // ============================================================
    // Private Helpers
    // ============================================================

    private static double CalculateBmr(double weight, double height, int age, string gender)
    {
        // Male BMR = 10×weight(kg) + 6.25×height(cm) − 5×age + 5
        // Female BMR = 10×weight + 6.25×height − 5×age − 161
        double baseCalc = (10 * weight) + (6.25 * height) - (5 * age);

        return gender.Equals("Male", StringComparison.OrdinalIgnoreCase)
            ? baseCalc + 5
            : baseCalc - 161;
    }

    private static double CalculateTdee(double bmr, ActivityLevel activityLevel)
    {
        return activityLevel switch
        {
            ActivityLevel.Rookie => bmr * 1.2,
            ActivityLevel.Beginner => bmr * 1.375,
            ActivityLevel.Intermediate => bmr * 1.55,
            ActivityLevel.Advance => bmr * 1.725,
            ActivityLevel.TrueBeast => bmr * 1.9,
            _ => bmr * 1.2
        };
    }

    private static double CalculateCalorieTarget(double tdee, Goal goal)
    {
        return goal switch
        {
            Goal.LoseWeight => tdee - 500,
            Goal.GainWeight => tdee + 300,
            Goal.GainMoreFlexible => tdee + 150,
            Goal.GetFitter => tdee,
            Goal.LearnTheBasic => tdee,
            _ => tdee
        };
    }

    private static FitnessStatus ClassifyStatus(double calorieTarget)
    {
        if (calorieTarget <= 1800)
            return FitnessStatus.Weak;

        if (calorieTarget <= 2500)
            return FitnessStatus.Normal;

        return FitnessStatus.Hard;
    }
}
