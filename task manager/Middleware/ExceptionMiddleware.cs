using System.Net;
using System.Text.Json;
using task_manager.Models;

namespace task_manager.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred at {Path} with method {Method}",
                   context.Request.Path,
                   context.Request.Method);
                await HandleException(context, ex);
            }
        }

        private static async Task HandleException(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            int statusCode;
            string message;

            switch (ex)
            {
                case UnauthorizedAccessException:
                    statusCode = StatusCodes.Status401Unauthorized;
                    message = ex.Message;
                    break;

                case KeyNotFoundException:
                    statusCode = StatusCodes.Status404NotFound;
                    message = ex.Message;
                    break;

                case FluentValidation.ValidationException validationEx:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = string.Join(", ", validationEx.Errors.Select(e => e.ErrorMessage));
                    break;

                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    message = "Something went wrong";
                    break;
            }

            context.Response.StatusCode = statusCode;

            var response = new ApiResponse<string>
            {
                Success = false,
                Message = message,
                Data = null
            };

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}