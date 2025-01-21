using IdentityManager.Exceptions;
using IdentityManager.Models;
using IdentityManager.Models.Enums;
using System.Text.Json;

namespace IdentityManager.Middlewares
{
    public class ExceptionHandling
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandling> _logger;

        public ExceptionHandling(RequestDelegate next, ILogger<ExceptionHandling> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (CustomException ex)
            {
                _logger.LogError($"Exception: {ex.Message}");
                await HandleCustomExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected exception: {ex.Message}");
                await HandleGeneralExceptionAsync(context, ex);
            }
        }

        private Task HandleCustomExceptionAsync(HttpContext context, CustomException ex)
        {
            var errorResponse = new ErrorResponse
            {
                ErrorCode = ex.ErrorCode,
                Message = ex.Message
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }

        private Task HandleGeneralExceptionAsync(HttpContext context, Exception ex)
        {
            var errorResponse = new ErrorResponse
            {
                ErrorCode = 409,
                Message = "Conflict"
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}