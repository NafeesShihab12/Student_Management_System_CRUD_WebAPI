using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace StudentManagementSystem.API.Middlewares
{


        public class RequestLoggingMiddleware
        {
            private readonly RequestDelegate _next;
            private readonly ILogger<RequestLoggingMiddleware> _logger;

            public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
            {
                _next = next;
                _logger = logger;
            }

            public async Task Invoke(HttpContext context)
            {
                // Log request details
                _logger.LogInformation($"Request {context.Request.Method}: {context.Request.Path}");

                // Call the next middleware in the pipeline
                await _next(context);
            }
        }

        public static class RequestLoggingMiddlewareExtensions
        {
            public static IApplicationBuilder UseRequestLoggingMiddleware(this IApplicationBuilder builder)
            {
                return builder.UseMiddleware<RequestLoggingMiddleware>();
            }
        }


        public class ErrorHandlingMiddleware
        {
            private readonly RequestDelegate _next;
            private readonly ILogger<ErrorHandlingMiddleware> _logger;

            public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
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
                catch (Exception ex)
                {
                    // Log the error
                    _logger.LogError(ex, "An unhandled exception occurred.");

                    // Set the status code to 500
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                    // Return an error message
                    await context.Response.WriteAsync("Internal server error");
                }
            }
        }

        public static class ErrorHandlingMiddlewareExtensions
        {
            public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
            {
                return builder.UseMiddleware<ErrorHandlingMiddleware>();
            }
        }

    

}
