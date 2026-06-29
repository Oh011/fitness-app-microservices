using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Shared.Results;
using NutritionService.Application.Abstractions.Services;

namespace NutritionService.Infrastructure.Services;

internal class FceServiceClient : IFceServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<FceServiceClient> _logger;

    public FceServiceClient(HttpClient httpClient, ILogger<FceServiceClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<Result<CalorieTargetDto>> GetCalorieTargetAsync(int userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/v1/fitness/metrics/{userId}");
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound || response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return Result<CalorieTargetDto>.NotFound("FCE_METRICS_NOT_CALCULATED");

                return Result<CalorieTargetDto>.Infrastructure("FCE service returned an error.");
            }

            var fceResponse = await response.Content.ReadFromJsonAsync<FceApiResponse>();
            if (fceResponse?.Data is null)
                return Result<CalorieTargetDto>.NotFound("FCE_METRICS_NOT_CALCULATED");

            return Result<CalorieTargetDto>.Success(new CalorieTargetDto
            {
                TargetCalories = fceResponse.Data.CalorieTarget,
                Goal = fceResponse.Data.Goal ?? string.Empty,
                ActivityLevel = fceResponse.Data.ActivityLevel ?? string.Empty
            });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "FCE service unavailable");
            return Result<CalorieTargetDto>.Infrastructure("SRV_SERVICE_UNAVAILABLE");
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "FCE service timeout");
            return Result<CalorieTargetDto>.Infrastructure("SRV_SERVICE_UNAVAILABLE");
        }
    }

    private class FceApiResponse
    {
        [JsonPropertyName("data")]
        public FceData? Data { get; set; }
    }

    private class FceData
    {
        [JsonPropertyName("calorieTarget")]
        public int CalorieTarget { get; set; }

        [JsonPropertyName("goal")]
        public string? Goal { get; set; }

        [JsonPropertyName("activityLevel")]
        public string? ActivityLevel { get; set; }
    }
}
