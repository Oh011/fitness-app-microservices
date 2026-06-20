
using Authenticore.Shared.Errors;


namespace Authenticore.Shared.Results
{

    public class Result
    {
        public bool IsSuccess { get; }
        public string? Message { get; }

        public SuccessType? Status { get; }
        public ErrorType? Error { get; }

        public Dictionary<string, List<ValidationErrorDetail>>? ValidationErrors { get; }

        protected Result(
            bool isSuccess,
            string? message,
            SuccessType? successType,
            ErrorType? errorType,
            Dictionary<string, List<ValidationErrorDetail>>? validationErrors)
        {
            IsSuccess = isSuccess;
            Message = message;
            Status = successType;
            Error = errorType;
            ValidationErrors = validationErrors;
        }

        // =========================
        // SUCCESS
        // =========================

        public static Result Success(string? message = null)
            => new(true, message, Results.SuccessType.OK , null, null);

        public static Result Created(string? message = null)
            => new(true, message, Results.SuccessType.Created, null, null);

        public static Result NoContent(string? message = null)
            => new(true, message, Results.SuccessType.NoContent, null, null);

        // =========================
        // ERRORS
        // =========================


        public static Result FromResult<TOther>(Result<TOther> result)
        {
            ArgumentNullException.ThrowIfNull(result);

            if (result.IsSuccess)
            {
                return result.Status switch
                {
                    SuccessType.Created =>
                        Result.Created( result.Message),

                    SuccessType.NoContent =>
                        Result.NoContent(result.Message),

                    _ =>
                        Result.Success( result.Message)
                };
            }

            return result.Error switch
            {
                ErrorType.Validation when result.ValidationErrors is not null =>
                       Result.Validation(result.ValidationErrors, result.Message),

                ErrorType.Validation =>
                    Result.Validation(result.Message ?? "Validation failed."),

                ErrorType.NotFound =>
                    Result.NotFound(result.Message),

                ErrorType.Conflict =>
                    Result.Conflict(result.Message),

                ErrorType.Unauthorized =>
                    Result.Unauthorized(result.Message),

                ErrorType.Forbidden =>
                    Result.Forbidden(result.Message),

                ErrorType.Infrastructure =>
                    Result.Infrastructure(result.Message),

                ErrorType.Internal =>
                    Result.Failure(result.Message),

                _ =>
                    Result.Failure(result.Message)
            };
        }


        public static  Result Failure(string? message = null)
            => new(false, message, null, ErrorType.Internal, null);

        public static Result NotFound(string? message = null)
            => new(false, message, null, ErrorType.NotFound, null);

        public static Result Conflict(string? message = null)
            => new(false, message, null, ErrorType.Conflict, null);

        public static Result Unauthorized(string? message = null)
            => new(false, message, null, ErrorType.Unauthorized, null);

        public static Result Forbidden(string? message = null)
            => new(false, message, null, ErrorType.Forbidden, null);

        public static Result Infrastructure(string? message = null)
            => new(false, message, null, ErrorType.Infrastructure, null);

        // =========================
        // VALIDATION
        // =========================

        public static Result Validation(
            Dictionary<string, List<ValidationErrorDetail>> validationErrors,
            string? message = "Validation errors occurred.")
            => new(false, message, null,    ErrorType.Validation, validationErrors);


        public static Result Validation(string message)
    => new(false, message, null, ErrorType.Validation, null);
    }






}