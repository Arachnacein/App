using BudgetManager.Exceptions;

namespace BudgetManager.Utils
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

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            string message;
            int statusCode;

            switch (exception)
            {
                case NullPointerException:
                        statusCode = StatusCodes.Status409Conflict;
                        message = "Data not found.";
                    break;
                case BadStringLengthException:
                        statusCode = StatusCodes.Status409Conflict;
                        message = "Invalid string length.";
                    break;
                case BadValueException:
                        statusCode = StatusCodes.Status409Conflict;
                        message = "Invalid value provided.";
                    break;
                default:
                        statusCode = StatusCodes.Status500InternalServerError;
                        message = "An unexpected error occurred.";
                    break;
            }

            context.Response.StatusCode = statusCode;
            _logger.LogError(exception, message);
            return context.Response.WriteAsync(new { error = message }.ToString());
        }
    }
}
