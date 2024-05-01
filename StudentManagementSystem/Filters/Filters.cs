using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace StudentManagementSystem.API.Filters
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        private readonly ILogger<AuthorizationFilter> _logger;

        public AuthorizationFilter(ILogger<AuthorizationFilter> logger)
        {
            _logger = logger;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Authorization logic
            if (!IsAuthorized(context))
            {
                _logger.LogWarning("Unauthorized access attempt: {Path}", context.HttpContext.Request.Path);
                context.Result = new ForbidResult();
            }
        }

        private bool IsAuthorized(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                var headerValue = authorizationHeader.ToString();
                if (headerValue.StartsWith("Bearer token1212"))
                {
                    return true;
                }
            }

            return false;
        }

        }


    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            // Log the exception
            _logger.LogError(context.Exception, "An unhandled exception occurred: {Path}", context.HttpContext.Request.Path);

            // Set the status code to 500
            context.HttpContext.Response.StatusCode = 500;

            // Return an error response
            context.Result = new JsonResult(new
            {
                error = new
                {
                    message = "Internal server error"
                }
            });
        }
    }
}
