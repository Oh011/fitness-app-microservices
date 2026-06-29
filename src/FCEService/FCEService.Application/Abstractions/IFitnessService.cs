using FCEService.Application.DTOs;
using FCEService.Domain.Enums;
using Shared.Results;

namespace FCEService.Application.Abstractions;

public interface IFitnessService
{
    // 3.1: Submit weight, goal & activity stats
    Task<Result> SubmitWeightGoalActivityAsync(WeightGoalActivityRequest request);

    // 3.2: Calculate BMR, TDEE & Calorie Target
    Task<Result<CalculateResponse>> CalculateAsync(CalculateRequest request);

    // 3.3: Assign fitness plan
    Task<Result<AssignPlanResponse>> AssignPlanAsync(CalculateRequest request);

    // 3.4: Get fitness metrics
    Task<Result<FitnessMetricsResponse>> GetMetricsAsync(int userId);

    // 3.5: Get fitness stats
    Task<Result<FitnessStatsResponse>> GetStatsAsync(int userId);

    // 3.6: Recalculate metrics & reassign plan
    Task<Result<RecalculateResponse>> RecalculateAsync(int userId, RecalculateRequest? request);

    // 3.7: Get fitness plan configurations (paginated)
    Task<Result<PlanConfigListResponse>> GetPlanConfigsAsync(Goal? goal, FitnessStatus? status, int page = 1, int pageSize = 20);

    // 3.8: Get specific plan configuration
    Task<Result<PlanConfigDto>> GetPlanByIdAsync(int planId);
}
