using System;

namespace GR.Models
{
    /// <summary>
    /// Describes the result of a request.
    /// </summary>
    public class RequestResult
    {
        public bool WasSuccessful { get; set; }
        public string[] Messages { get; set; }
        public string[] Warnings { get; set; }
        public string[] Errors { get; set; }

        public static RequestResult Success(string message = null)
            => new RequestResult
            {
                WasSuccessful = true,
                Messages = message == null ? null : new[] { message }
            };

        public static RequestResult<T> Success<T>(T data, string message = null)
            => new RequestResult<T>
            {
                WasSuccessful = true,
                Messages = message == null ? null : new[] { message },
                Data = data,
            };

        public static RequestResult Failure(string error)
            => new RequestResult
            {
                WasSuccessful = false,
                Errors = error == null ? null : new[] { error }
            };

        public static RequestResult<T> Failure<T>(Exception ex)
            => Failure<T>(default(T), $"{ex.GetType().Name}: {ex.Message}");

        public static RequestResult<T> Failure<T>(T data, string error)
            => new RequestResult<T>
            {
                WasSuccessful = false,
                Errors = error == null ? null : new[] { error },
                Data = data,
            };
    }

    /// <summary>
    /// Describes the result of a request that returns data.
    /// </summary>
    /// <typeparam name="T">The type of data returned by the request.</typeparam>
    public class RequestResult<T> : RequestResult
    {
        public T Data { get; set; }
    }
}
