using HR_Medical_Records.DTOs.BaseResponse;
using HR_Medical_Records.Middleware.Exceptions;
using System.Net;
using System.Text.Json;

namespace HR_Medical_Records.Middleware
{
    /// <summary>
    /// Middleware for handling errors globally in the application.
    /// Catches exceptions and returns appropriate HTTP responses with error details.
    /// </summary>
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;

        public ErrorMiddleware(RequestDelegate next, IHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        /// <summary>
        /// Invokes the middleware to handle the request and any potential errors.
        /// </summary>
        /// <param name="context">The HTTP context of the request.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                var baseResponse = new BaseResponse<string>
                {
                    Success = false,
                    Data = null
                };

                switch (error)
                {
                    case ExceptionBadRequestClient e:
                        // 400 Bad Request
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        baseResponse.Message = e.Message;
                        baseResponse.Code = response.StatusCode;
                        baseResponse.Exception = "BadRequest";
                        break;

                    case KeyNotFoundException e:
                        // 404 Not Found
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        baseResponse.Message = e.Message;
                        baseResponse.Code = response.StatusCode;
                        baseResponse.Exception = "KeyNotFoundException";
                        break;

                    default:
                        // 500 Internal Server Error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        baseResponse.Message = "An unexpected error occurred";
                        baseResponse.Code = response.StatusCode;
                        baseResponse.Exception = "Exception";
                        break;
                }

                if (error?.InnerException != null)
                {
                    baseResponse.Exception = $"{error.Message} | Inner Exception: {error.InnerException.Message}";
                }

                var jsonResponse = JsonSerializer.Serialize(baseResponse);

                await response.WriteAsync(jsonResponse);
            }
        }
    }
}
