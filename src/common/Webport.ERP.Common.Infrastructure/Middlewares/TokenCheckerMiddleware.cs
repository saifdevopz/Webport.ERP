using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace Webport.ERP.Common.Infrastructure.Middlewares;

public class TokenCheckerMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        string requestPath = context.Request.Path.Value!;

        if (requestPath.Contains("identity/tokens", StringComparison.InvariantCultureIgnoreCase)
            || requestPath.Contains("scalar", StringComparison.InvariantCultureIgnoreCase)
            || requestPath.Contains("openapi", StringComparison.InvariantCultureIgnoreCase)
            || requestPath.Equals("/", StringComparison.Ordinal))
        {
            await next(context);
        }
        else
        {
            if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader) || string.IsNullOrWhiteSpace(authHeader))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Access denied: Missing Authorization header.");
                return;
            }

            var token = authHeader.ToString().Split(" ").LastOrDefault();

            if (string.IsNullOrWhiteSpace(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Access denied: Invalid token format.");
                return;
            }

            try
            {
                if (IsTokenExpired(token))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Token expired.");
                    return;
                }

                await next(context);
            }
            catch (MissingFieldException)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Access denied: Invalid or unreadable token.");
            }
        }
    }

    public static bool IsTokenExpired(string token)
    {
        JwtSecurityTokenHandler handler = new();

        // Validate if the token is a valid JWT token
        if (!handler.CanReadToken(token))
        {
            throw new ArgumentException("Invalid JWT token");
        }

        // Read the token
        JwtSecurityToken jwtToken = handler.ReadJwtToken(token);

        // Extract the exp claim (expiration time)
        string exp = jwtToken.Claims.First(claim => claim.Type == "exp").Value;

        // Convert exp claim to DateTime
        DateTime expirationTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(exp, CultureInfo.InvariantCulture)).UtcDateTime;

        // Compare expiration time with the current time
        return expirationTime < DateTime.UtcNow;
    }
}
