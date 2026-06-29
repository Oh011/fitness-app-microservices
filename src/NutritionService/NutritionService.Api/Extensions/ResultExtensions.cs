using Microsoft.AspNetCore.Mvc;
using Shared.Errors;
using Shared.Responses;
using Shared.Results;
using System.Net;

namespace NutritionService.Api.Extensions;

public static class ResultExtensions
{
    private static HttpStatusCode MapError(ErrorType? error)
    {
        return error switch
        {
            ErrorType.NotFound => HttpStatusCode.NotFound,
            ErrorType.Unauthorized => HttpStatusCode.Unauthorized,
            ErrorType.Forbidden => HttpStatusCode.Forbidden,
            ErrorType.Conflict => HttpStatusCode.Conflict,
            ErrorType.Validation => HttpStatusCode.BadRequest,
            ErrorType.Infrastructure => HttpStatusCode.ServiceUnavailable,
            _ => HttpStatusCode.InternalServerError
        };
    }

    private static HttpStatusCode MapSuccess(SuccessType? successType) => successType switch
    {
        SuccessType.Created => HttpStatusCode.Created,
        SuccessType.NoContent => HttpStatusCode.NoContent,
        _ => HttpStatusCode.OK
    };

    public static ActionResult<ApiResponse> ToApiResponse<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            var successResponse = result.Status switch
            {
                SuccessType.Created => ApiResponse<T>.Created(result.Value!, result.Message),
                SuccessType.NoContent => ApiResponse<T>.NoContent(result.Message),
                _ => ApiResponse<T>.Ok(result.Value!, result.Message)
            };

            return new ObjectResult(successResponse)
            {
                StatusCode = (int)MapSuccess(result.Status)
            };
        }

        var errorResponse = result.ValidationErrors is not null && result.ValidationErrors.Any()
            ? ApiResponse<T>.Validation(result.ValidationErrors, result.Message)
            : result.Error switch
            {
                ErrorType.NotFound => ApiResponse<T>.NotFound(result.Message),
                ErrorType.Conflict => ApiResponse<T>.Conflict(result.Message),
                ErrorType.Unauthorized => ApiResponse<T>.Unauthorized(result.Message),
                ErrorType.Forbidden => ApiResponse<T>.Forbidden(result.Message),
                ErrorType.Validation => ApiResponse<T>.Validation(result.ValidationErrors, result.Message ?? "Validation failed"),
                ErrorType.Infrastructure => ApiResponse<T>.Infrastructure(result.Message),
                _ => ApiResponse<T>.Internal(result.Message)
            };

        return new ObjectResult(errorResponse)
        {
            StatusCode = (int)MapError(result.Error)
        };
    }

    public static ActionResult<ApiResponse> ToApiResponse(this Result result)
    {
        if (result.IsSuccess)
        {
            var success = result.Status switch
            {
                SuccessType.Created => ApiResponse.Created(result.Message),
                SuccessType.NoContent => ApiResponse.NoContent(result.Message),
                _ => ApiResponse.Ok(result.Message)
            };

            return new ObjectResult(success)
            {
                StatusCode = (int)MapSuccess(result.Status)
            };
        }

        var error = result.ValidationErrors is not null && result.ValidationErrors.Any()
            ? ApiResponse.Validation(result.ValidationErrors, result.Message)
            : result.Error switch
            {
                ErrorType.NotFound => ApiResponse.NotFound(result.Message),
                ErrorType.Conflict => ApiResponse.Conflict(result.Message),
                ErrorType.Unauthorized => ApiResponse.Unauthorized(result.Message),
                ErrorType.Forbidden => ApiResponse.Forbidden(result.Message),
                ErrorType.Validation => ApiResponse.Validation(result.ValidationErrors, result.Message ?? "Validation failed"),
                ErrorType.Infrastructure => ApiResponse.Infrastructure(result.Message),
                _ => ApiResponse.Internal(result.Message)
            };

        return new ObjectResult(error)
        {
            StatusCode = (int)MapError(result.Error)
        };
    }
}
