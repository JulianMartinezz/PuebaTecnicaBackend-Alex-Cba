using HR_Medical_Records.DTOs.BaseResponse;
using HR_Medical_Records.Middleware.Exceptions;
using System.Net;
using System.Text.Json;

namespace HR_Medical_Records.Middleware
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

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
                        break;

                    case KeyNotFoundException e:
                        // 404 Not Found
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        baseResponse.Message = e.Message;
                        baseResponse.Code = response.StatusCode;
                        break;

                    default:
                        // 500 Internal Server Error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        baseResponse.Message = "An unexpected error occurred.";
                        baseResponse.Code = response.StatusCode;
                        break;
                }

                if (error?.InnerException != null)
                {
                    baseResponse.Exception = $"{error.Message} | Inner Exception: {error.InnerException.Message}";
                }
                else
                {
                    baseResponse.Exception = error.Message;
                }

                var jsonResponse = JsonSerializer.Serialize(baseResponse);

                await response.WriteAsync(jsonResponse);
            }
        }
    }
}
