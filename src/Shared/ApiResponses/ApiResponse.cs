

using Shared.Errors;

namespace Shared.Responses
{
    public class ApiResponse
    {
        public bool Success { get; init; } = true;
        public int StatusCode { get; init; }
        public string? Message { get; init; }
        public Dictionary<string, List<ValidationErrorDetail>>? Errors { get; init; }
        public DateTime TimeStap { get; init; } = DateTime.UtcNow;

// =========================
// SUCCESS FACTORIES
// =========================

public static ApiResponse Ok(string? message = null)
    => new()
    {
        Success = true,
        StatusCode = 200,
        Message = message
    };

        public static ApiResponse Created(string? message = null)
            => new()
            {
                Success = true,
                StatusCode = 201,
                Message = message
            };

        public static ApiResponse NoContent(string? message = null)
            => new()
            {
                Success = true,
                StatusCode = 204,
                Message = message
            };

        // =========================
        // ERROR FACTORIES
        // =========================

        public static ApiResponse Validation(
            Dictionary<string, List<ValidationErrorDetail>> ? errors,
            string? message = "Validation failed")
            => new()
            {
                Success = false,
                StatusCode = 400,
                Message = message,
                Errors = errors
            };

        public static ApiResponse NotFound(string? message = "Resource not found")
            => new()
            {
                Success = false,
                StatusCode = 404,
                Message = message
            };

        public static ApiResponse Conflict(string? message = "Conflict occurred")
            => new()
            {
                Success = false,
                StatusCode = 409,
                Message = message
            };

        public static ApiResponse Unauthorized(string? message = "Unauthorized")
            => new()
            {
                Success = false,
                StatusCode = 401,
                Message = message
            };

        public static ApiResponse Forbidden(string? message = "Forbidden")
            => new()
            {
                Success = false,
                StatusCode = 403,
                Message = message
            };

        public static ApiResponse Infrastructure(string? message = "Infrastructure error")
            => new()
            {
                Success = false,
                StatusCode = 503,
                Message = message
            };

        public static ApiResponse Internal(string? message = "Internal server error")
            => new()
            {
                Success = false,
                StatusCode = 500,
                Message = message
            };


}

    public class ApiResponse<T> : ApiResponse
    {
        public T? Data { get; init; }


// =========================
// GENERIC SUCCESS FACTORIES
// =========================

        public static ApiResponse<T> Ok(T ? data, string? message = null)
            => new()
            {
                Success = true,
                StatusCode = 200,
                Data = data,
                Message = message
            };

        public static ApiResponse<T> Created(T ? data, string? message = null)
            => new()
            {
                Success = true,
                StatusCode = 201,
                Data = data,
                Message = message
            };

        public static ApiResponse<T> NoContent(string? message = null)
            => new()
            {
                Success = true,
                StatusCode = 204,
                Data = default,
                Message = message
            };

        // =========================
        // GENERIC ERROR FACTORIES
        // =========================

        public static ApiResponse<T> Validation(
            Dictionary<string, List<ValidationErrorDetail>> ? errors,
            string? message = "Validation failed")
            => new()
            {
                Success = false,
                StatusCode = 400,
                Errors = errors,
                Message = message
            };

        public static ApiResponse<T> NotFound(string? message = "Resource not found")
            => new()
            {
                Success = false,
                StatusCode = 404,
                Message = message
            };

        public static ApiResponse<T> Conflict(string? message = "Conflict occurred")
            => new()
            {
                Success = false,
                StatusCode = 409,
                Message = message
            };

        public static ApiResponse<T> Unauthorized(string? message = "Unauthorized")
            => new()
            {
                Success = false,
                StatusCode = 401,
                Message = message
            };

        public static ApiResponse<T> Forbidden(string? message = "Forbidden")
            => new()
            {
                Success = false,
                StatusCode = 403,
                Message = message
            };

        public static ApiResponse<T> Infrastructure(string? message = "Infrastructure error")
            => new()
            {
                Success = false,
                StatusCode = 503,
                Message = message
            };

        public static ApiResponse<T> Internal(string? message = "Internal server error")
            => new()
            {
                Success = false,
                StatusCode = 500,
                Message = message
            };


}


}
