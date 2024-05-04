using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.API.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthenticationMiddleware> _logger;
        private readonly string _secretKey;

        public AuthenticationMiddleware(RequestDelegate next, ILogger<AuthenticationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _secretKey = "token1212";
        }

        public async Task Invoke(HttpContext context)
        {
            // Check for authentication logic
            if (!IsAuthenticated(context))
            {
                _logger.LogWarning("Unauthorized access attempt: {Path}", context.Request.Path);
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            await _next(context);
        }

        private bool IsAuthenticated(HttpContext context)
        {
            // Extract JWT token from request headers
            if (context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                var token = authorizationHeader.ToString().Replace("Bearer ", "");

                // Validate JWT token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secretKey);

                try
                {
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    return true;
                }
                catch (Exception ex)
                {
                    // Log any validation errors
                    _logger.LogError(ex, "JWT token validation failed");
                    return false;
                }
            }

            return false;
        }
    }
}
