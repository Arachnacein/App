using BudgetManager.Exceptions;
using BudgetManager.Exceptions.IncomeExceptions;
using BudgetManager.Exceptions.PatternExceptions;
using BudgetManager.Exceptions.TransactionExceptions;
using System.Text.Json;

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
                        message = "Object is Null.";
                    break;
                case BadStringLengthException:
                        statusCode = StatusCodes.Status409Conflict;
                        message = "Invalid string length.";
                    break;
                case BadValueException:
                        statusCode = StatusCodes.Status409Conflict;
                        message = "Invalid value provided.";
                    break;                
                case TransactionNotFoundException:
                        statusCode = StatusCodes.Status409Conflict;
                        message = "Transaction not found.";
                    break;               
                case PatternNotFoundException:
                        statusCode = StatusCodes.Status409Conflict;
                        message = "Pattern not found.";
                    break;                 
                case IncomeNotFoundException:
                        statusCode = StatusCodes.Status409Conflict;
                        message = "Income not found.";
                    break;                
                case MonthPatternAlreadyExistsException:
                        statusCode = StatusCodes.Status409Conflict;
                        message = "Month pattern already exists.";
                    break;
                default:
                        statusCode = StatusCodes.Status500InternalServerError;
                        message = "An unexpected error occurred.";
                    break;
            }

            context.Response.StatusCode = statusCode;
            _logger.LogError(exception, message);
            var result = JsonSerializer.Serialize(new { error = message });

            return context.Response.WriteAsync(result);
        }
    }
}
