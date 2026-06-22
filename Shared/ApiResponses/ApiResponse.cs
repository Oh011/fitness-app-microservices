

using Shared.Errors;

namespace Shared.Responses
{
    public class ApiResponse
    {

        public bool Success { get; init; } = true;
        public int StatusCode { get; init; }
        public string? Message { get; init; }

        public Dictionary<string, List<ValidationErrorDetail>> Errors { get; init; }


        public DateTime TimeStap {  get; init; }=DateTime.Now;



    }



    public class ApiResponse<T>
    {

        public bool Success { get; init; } = true;
        public int StatusCode { get; init; }
        public string? Message { get; init; }


        public T ? Data { get; init; }

        public Dictionary<string, List<ValidationErrorDetail>> ? Errors { get; init; }


        public DateTime TimeStap { get; init; } = DateTime.Now;



    }

}
