
using Shared.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Results
{
    public class Result<T> : Result
    {
        public T? Value { get; }

        private Result(
            bool isSuccess,
            T? value,
            string? message,
            SuccessType? successType,
            ErrorType? errorType,
            Dictionary<string, List<ValidationErrorDetail>>? validationErrors)
            : base(isSuccess, message, successType, errorType, validationErrors)
        {
            Value = value;
        }



        public static Result<T> FromResult(Result result)
        {
            ArgumentNullException.ThrowIfNull(result);

            if (result.IsSuccess)
            {
                return result.Status switch
                {
                    SuccessType.Created =>
                        Result<T>.Created(default!, result.Message),

                    SuccessType.NoContent =>
                        Result<T>.NoContent(result.Message),

                    _ =>
                        Result<T>.Success(default!, result.Message)
                };
            }

            return result.Error switch
            {
             ErrorType.Validation when result.ValidationErrors is not null =>
                    Result<T>.Validation(result.ValidationErrors, result.Message),

                ErrorType.Validation =>
                    Result<T>.Validation(result.Message ?? "Validation failed."),

                ErrorType.NotFound =>
                    Result<T>.NotFound(result.Message),

                ErrorType.Conflict =>
                    Result<T>.Conflict(result.Message),

                ErrorType.Unauthorized =>
                    Result<T>.Unauthorized(result.Message),

                ErrorType.Forbidden =>
                    Result<T>.Forbidden(result.Message),

                ErrorType.Infrastructure =>
                    Result<T>.Infrastructure(result.Message),

                ErrorType.Internal =>
                    Result<T>.Failure(result.Message),

                _ =>
                    Result<T>.Failure(result.Message)
            };
        }


        public static Result<T> FromResult<TOther>(Result<TOther> result)
        {
            ArgumentNullException.ThrowIfNull(result);

            if (result.IsSuccess)
            {
                return result.Status switch
                {
                    SuccessType.Created =>
                        Result<T>.Created(default!, result.Message),

                    SuccessType.NoContent =>
                        Result<T>.NoContent(result.Message),

                    _ =>
                        Result<T>.Success(default!, result.Message)
                };
            }

            return result.Error switch
            {
                ErrorType.Validation when result.ValidationErrors is not null =>
                       Result<T>.Validation(result.ValidationErrors, result.Message),

                ErrorType.Validation =>
                    Result<T>.Validation(result.Message ?? "Validation failed."),

                ErrorType.NotFound =>
                    Result<T>.NotFound(result.Message),

                ErrorType.Conflict =>
                    Result<T>.Conflict(result.Message),

                ErrorType.Unauthorized =>
                    Result<T>.Unauthorized(result.Message),

                ErrorType.Forbidden =>
                    Result<T>.Forbidden(result.Message),

                ErrorType.Infrastructure =>
                    Result<T>.Infrastructure(result.Message),

                ErrorType.Internal =>
                    Result<T>.Failure(result.Message),

                _ =>
                    Result<T>.Failure(result.Message)
            };
        }


        // =========================
        // SUCCESS
        // =========================

        public static Result<T> Success(T value, string? message = null)
            => new(true, value, message, Results.SuccessType.OK, null, null);

        public static Result<T> Created(T value, string? message = null)
            => new(true, value, message, Results.SuccessType.Created, null, null);

        public static Result<T> NoContent(string? message = null)
            => new(true, default, message, Results.SuccessType.NoContent, null, null);

        // =========================
        // ERRORS
        // =========================

        public static Result<T> Failure(string? message = null)
            => new(false, default, message, null, ErrorType.Internal, null);

        public static Result<T> NotFound(string? message = null)
            => new(false, default, message, null, ErrorType.NotFound, null);

        public static Result<T> Conflict(string? message = null)
            => new(false, default, message, null, ErrorType.Conflict, null);

        public static Result<T> Unauthorized(string? message = null)
            => new(false, default, message, null, ErrorType.Unauthorized, null);

        public static Result<T> Forbidden(string? message = null)
            => new(false, default, message, null, ErrorType.Forbidden, null);

        public static Result<T> Infrastructure(string? message = null)
            => new(false, default, message, null, ErrorType.Infrastructure, null);

        // =========================
        // VALIDATION
        // =========================

        public static Result<T> Validation(
            Dictionary<string, List<ValidationErrorDetail>> validationErrors,
            string? message = "Validation errors occurred.")
            => new(false, default, message, null, ErrorType.Validation, validationErrors);



        public static Result<T> Validation(string message)
         => new(false,default, message, null,   ErrorType.Validation, null);
    }
}
