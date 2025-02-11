using CarParkInfo.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace CarParkInfo.API.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<JwtMiddleware> _logger;

    public JwtMiddleware(RequestDelegate next, ILogger<JwtMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IAuthService authService)
    {
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        _logger.LogInformation($"Auth header: {authHeader}");

        var token = authHeader?.Split(" ").Last();
        if (token != null)
        {
            _logger.LogInformation($"Extracted token: {token[..Math.Min(token.Length, 20)]}...");

            try
            {
                var isValid = await authService.ValidateTokenAsync(token);
                _logger.LogInformation($"Token validation result: {isValid}");

                if (isValid)
                {
                    var userId = authService.GetUserIdFromToken(token);
                    _logger.LogInformation($"Extracted userId: {userId}");
                    context.Items["UserId"] = userId;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing token: {ex.Message}");
            }
        }
        else
        {
            _logger.LogWarning("No token found in request");
        }

        await _next(context);
    }
}