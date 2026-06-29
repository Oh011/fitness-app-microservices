using FCEService.Application.Abstractions;
using FCEService.Application.DTOs;
using FCEService.Domain.Enums;
using FCEService.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Responses;

namespace FCEService.Api.Controllers;

[ApiController]
[Route("api/v1/fitness")]
[Authorize]
public class FitnessController : ControllerBase
{
    private readonly IFitnessService _fitnessService;

    public FitnessController(IFitnessService fitnessService)
    {
        _fitnessService = fitnessService;
    }

    /// <summary>
    /// 3.1 — Submit Weight, Goal &amp; Activity Stats
    /// </summary>
    [HttpPost("weight-goal-activity")]
    public async Task<ActionResult<ApiResponse>> SubmitWeightGoalActivity([FromBody] WeightGoalActivityRequest request)
    {
        var result = await _fitnessService.SubmitWeightGoalActivityAsync(request);
        return result.ToApiResponse();
    }

    /// <summary>
    /// 3.2 — Calculate BMR, TDEE &amp; Calorie Target
    /// </summary>
    [HttpPost("calculate")]
    public async Task<ActionResult<ApiResponse>> Calculate([FromBody] CalculateRequest request)
    {
        var result = await _fitnessService.CalculateAsync(request);
        return result.ToApiResponse();
    }

    /// <summary>
    /// 3.3 — Assign Fitness Plan
    /// </summary>
    [HttpPost("assign-plan")]
    public async Task<ActionResult<ApiResponse>> AssignPlan([FromBody] CalculateRequest request)
    {
        var result = await _fitnessService.AssignPlanAsync(request);
        return result.ToApiResponse();
    }

    /// <summary>
    /// 3.4 — Get Fitness Metrics
    /// </summary>
    [HttpGet("metrics/{userId}")]
    public async Task<ActionResult<ApiResponse>> GetMetrics(int userId)
    {
        var result = await _fitnessService.GetMetricsAsync(userId);
        return result.ToApiResponse();
    }

    /// <summary>
    /// 3.5 — Get Fitness Stats
    /// </summary>
    [HttpGet("stats/{userId}")]
    public async Task<ActionResult<ApiResponse>> GetStats(int userId)
    {
        var result = await _fitnessService.GetStatsAsync(userId);
        return result.ToApiResponse();
    }

    /// <summary>
    /// 3.6 — Recalculate Metrics &amp; Reassign Plan
    /// </summary>
    [HttpPut("recalculate/{userId}")]
    public async Task<ActionResult<ApiResponse>> Recalculate(int userId, [FromBody] RecalculateRequest? request)
    {
        var result = await _fitnessService.RecalculateAsync(userId, request);
        return result.ToApiResponse();
    }

    /// <summary>
    /// 3.7 — Get Fitness Plan Configurations
    /// </summary>
    [HttpGet("plan-configs")]
    public async Task<ActionResult<ApiResponse>> GetPlanConfigs(
        [FromQuery] Goal? goal,
        [FromQuery] FitnessStatus? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var result = await _fitnessService.GetPlanConfigsAsync(goal, status, page, pageSize);
        return result.ToApiResponse();
    }

    /// <summary>
    /// 3.8 — Get Specific Plan Configuration
    /// </summary>
    [HttpGet("plans/{planId}")]
    public async Task<ActionResult<ApiResponse>> GetPlanById(int planId)
    {
        var result = await _fitnessService.GetPlanByIdAsync(planId);
        return result.ToApiResponse();
    }
}
