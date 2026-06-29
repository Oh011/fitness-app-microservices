using Microsoft.EntityFrameworkCore;
using Shared.Results;
using NutritionService.Application.Abstractions.Services;
using NutritionService.Application.Features.Nutrition.Dtos;
using NutritionService.Domain.Entities;
using NutritionService.Infrastructure.Persistence.Context;

namespace NutritionService.Infrastructure.Services;

internal class NutritionService : INutritionService
{
    private readonly NutritionDbContext _context;
    private readonly IFceServiceClient _fceClient;

    public NutritionService(NutritionDbContext context, IFceServiceClient fceClient)
    {
        _context = context;
        _fceClient = fceClient;
    }

    public async Task<Result<RecommendationResponseDto>> GetRecommendationsAsync(
        int userId, string? mealType, int? maxCalories, int? minProtein, int page = 1, int pageSize = 20)
    {
        var calorieResult = await _fceClient.GetCalorieTargetAsync(userId);
        if (!calorieResult.IsSuccess)
            return Result<RecommendationResponseDto>.Validation("FCE_METRICS_NOT_CALCULATED");

        var calorieTarget = calorieResult.Value!.TargetCalories;

        var query = from meal in _context.Meals
                    join facts in _context.NutritionFacts on meal.Id equals facts.Id
                    select new { meal, facts };

        if (!string.IsNullOrEmpty(mealType))
            query = query.Where(x => x.meal.MealType == mealType);

        query = query.Where(x => x.facts.Calories <= calorieTarget);

        if (maxCalories.HasValue)
            query = query.Where(x => x.facts.Calories <= maxCalories.Value);

        if (minProtein.HasValue)
            query = query.Where(x => x.facts.Protein >= minProtein.Value);

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(x => x.meal.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dtos = items.Select(x => new MealDto(
            x.meal.Id, x.meal.MealPlanId, x.meal.Name, x.meal.MealType,
            x.meal.PrepTimeInMinutes, x.meal.Difficulty, x.meal.ImageUrl,
            x.facts.Calories, x.facts.Protein, x.facts.Carbs, x.facts.Fats,
            x.facts.Fiber, x.meal.TagsJson
        )).ToList();

        var response = new RecommendationResponseDto(calorieTarget, dtos);
        return Result<RecommendationResponseDto>.Success(response);
    }

    public async Task<Result<RecommendationResponseDto>> GetRecommendationsByUserIdAsync(
        int userId, int targetUserId, string? mealType, int? maxCalories, int? minProtein, int page = 1, int pageSize = 20)
    {
        var calorieResult = await _fceClient.GetCalorieTargetAsync(targetUserId);
        if (!calorieResult.IsSuccess)
            return Result<RecommendationResponseDto>.Validation("FCE_METRICS_NOT_CALCULATED");

        var calorieTarget = calorieResult.Value!.TargetCalories;

        var query = from meal in _context.Meals
                    join facts in _context.NutritionFacts on meal.Id equals facts.Id
                    select new { meal, facts };

        if (!string.IsNullOrEmpty(mealType))
            query = query.Where(x => x.meal.MealType == mealType);

        query = query.Where(x => x.facts.Calories <= calorieTarget);

        if (maxCalories.HasValue)
            query = query.Where(x => x.facts.Calories <= maxCalories.Value);

        if (minProtein.HasValue)
            query = query.Where(x => x.facts.Protein >= minProtein.Value);

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(x => x.meal.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dtos = items.Select(x => new MealDto(
            x.meal.Id, x.meal.MealPlanId, x.meal.Name, x.meal.MealType,
            x.meal.PrepTimeInMinutes, x.meal.Difficulty, x.meal.ImageUrl,
            x.facts.Calories, x.facts.Protein, x.facts.Carbs, x.facts.Fats,
            x.facts.Fiber, x.meal.TagsJson
        )).ToList();

        var response = new RecommendationResponseDto(calorieTarget, dtos);
        return Result<RecommendationResponseDto>.Success(response);
    }

    public async Task<Result<MealDetailDto>> GetMealDetailAsync(int id)
    {
        var query = from meal in _context.Meals
                    join facts in _context.NutritionFacts on meal.Id equals facts.Id
                    where meal.Id == id
                    select new MealDetailDto(
                        meal.Id, meal.MealPlanId, meal.Name, meal.MealType,
                        meal.PrepTimeInMinutes, meal.Difficulty, meal.ImageUrl,
                        facts.Calories, facts.Protein, facts.Carbs, facts.Fats,
                        facts.Fiber, meal.IngredientsJson, meal.InstructionsJson,
                        meal.VariationsJson, meal.AllergensJson, meal.TagsJson
                    );

        var dto = await query.FirstOrDefaultAsync();
        if (dto is null)
            return Result<MealDetailDto>.NotFound("RES_MEAL_NOT_FOUND");

        return Result<MealDetailDto>.Success(dto);
    }

    public async Task<Result<PaginatedResult<MealPlanDto>>> GetMealPlansAsync(int page = 1, int pageSize = 20)
    {
        var query = _context.MealPlans.Where(mp => true).OrderBy(mp => mp.Name);

        var totalCount = await query.CountAsync();
        var plans = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        var dtos = new List<MealPlanDto>();
        foreach (var plan in plans)
        {
            var items = await _context.MealPlanItems
                .Where(mpi => mpi.MealPlanId == plan.Id)
                .Join(_context.Meals, mpi => mpi.MealId, m => m.Id, (mpi, m) => new MealPlanItemDto(
                    mpi.Id, mpi.MealPlanId, mpi.MealId, m.Name, mpi.DayOfWeek, mpi.MealTime
                ))
                .ToListAsync();

            dtos.Add(new MealPlanDto(plan.Id, plan.Name, plan.Description, plan.CalorieTarget, items));
        }

        return Result<PaginatedResult<MealPlanDto>>.Success(
            new PaginatedResult<MealPlanDto>(page, pageSize, totalCount, dtos));
    }

    public async Task<Result<List<MealPlanDto>>> GetMealPlansByCaloriesAsync(int calories)
    {
        var tolerance = 200;
        var plans = await _context.MealPlans
            .Where(mp => mp.CalorieTarget >= calories - tolerance && mp.CalorieTarget <= calories + tolerance)
            .OrderBy(mp => mp.CalorieTarget)
            .ToListAsync();

        var dtos = new List<MealPlanDto>();
        foreach (var plan in plans)
        {
            var items = await _context.MealPlanItems
                .Where(mpi => mpi.MealPlanId == plan.Id)
                .Join(_context.Meals, mpi => mpi.MealId, m => m.Id, (mpi, m) => new MealPlanItemDto(
                    mpi.Id, mpi.MealPlanId, mpi.MealId, m.Name, mpi.DayOfWeek, mpi.MealTime
                ))
                .ToListAsync();

            dtos.Add(new MealPlanDto(plan.Id, plan.Name, plan.Description, plan.CalorieTarget, items));
        }

        return Result<List<MealPlanDto>>.Success(dtos);
    }
}
