using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace StudentManagementSystem.API.Filters
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        private readonly ILogger<AuthorizationFilter> _logger;
        private readonly JwtTokenValidator _tokenValidator;

        public AuthorizationFilter(ILogger<AuthorizationFilter> logger, JwtTokenValidator tokenValidator)
        {
            _logger = logger;
            _tokenValidator = tokenValidator;
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
            // Get the authorization header value
            if (context.HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                var token = authorizationHeader.ToString().Split(" ")[1]; // Extract the JWT token from the Authorization header

                // Validate the JWT token
                var isValidToken = _tokenValidator.ValidateToken(token);

                return isValidToken;
            }

            return true; 
        }

    }

    public class JwtTokenValidator
    {
        // Method to validate JWT tokens
        public bool ValidateToken(string token)
        {
            try
            {
                // Configure token validation parameters
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("token1212")),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true
                };

                // Validate the token
                tokenHandler.ValidateToken(token, validationParameters, out _);

                return true; // Token is valid
            }
            catch (Exception ex)
            {
                // Log any validation errors
                Console.WriteLine($"Token validation error: {ex.Message}");
                return false; // Token is invalid
            }
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
