//using System.IdentityModel.Tokens.Jwt;

//namespace ApiGateway.Middleware
//{
//    public class TokenCheckerMiddleware(RequestDelegate next)
//    {
//        public async Task InvokeAsync(HttpContext context)
//        {
//            string requestPath = context.Request.Path.Value!;
//            if (requestPath.Contains("identity/tokens", StringComparison.InvariantCultureIgnoreCase)
//                || requestPath.Contains("identity/tokens/refresh", StringComparison.InvariantCultureIgnoreCase)
//                || requestPath.Equals("/"))
//            {
//                await next(context);
//            }
//            else
//            {
//                var token = context.Request.Headers.Authorization.ToString().Split(" ").Last();
//                var res = IsTokenExpired(token);

//                if (res)
//                {
//                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
//                    await context.Response.WriteAsync("Token Expired.");
//                }

//                if (token == null)
//                {
//                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
//                    await context.Response.WriteAsync("Sorry, Access denied");
//                }
//                else
//                {
//                    await next(context);
//                }
//            }
//        }
//        public static bool IsTokenExpired(string token)
//        {
//            var handler = new JwtSecurityTokenHandler();

//            // Validate if the token is a valid JWT token
//            if (!handler.CanReadToken(token))
//            {
//                throw new ArgumentException("Invalid JWT token");
//            }

//            // Read the token
//            var jwtToken = handler.ReadJwtToken(token);

//            // Extract the exp claim (expiration time)
//            var exp = jwtToken.Claims.First(claim => claim.Type == "exp").Value;

//            // Convert exp claim to DateTime
//            var expirationTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(exp)).UtcDateTime;

//            // Compare expiration time with the current time
//            return expirationTime < DateTime.UtcNow;
//        }
//    }
//}
