using JsonPlaceHolderWrapperService.Models;
using System.Net;
using System.Text.Json;

namespace JsonPlaceHolderWrapperService.Helpers
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
            _logger.LogInformation("Handling request: {Method} {Path}", context.Request.Method, context.Request.Path);
            try
            {
                await _next(context); // Let pipeline continue
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred while processing request: {Method} {Path}", context.Request.Method, context.Request.Path);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = new ErrorResponse
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Internal Server Error",
                    Details = ex.Message
                };

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
            _logger.LogInformation("Finished handling request: {Method} {Path} with status code {StatusCode}", context.Request.Method, context.Request.Path, context.Response.StatusCode);
        }
    }
}