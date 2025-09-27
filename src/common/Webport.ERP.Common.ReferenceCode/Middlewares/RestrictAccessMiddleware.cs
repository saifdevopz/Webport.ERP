//using Microsoft.AspNetCore.Http;

//namespace BuildingBlocks.Infrastructure.Middlewares
//{
//    public class RestrictAccessMiddleware(RequestDelegate next)
//    {
//        public async Task InvokeAsync(HttpContext context)
//        {
//            var referrer = context.Request.Headers["Referrer"].FirstOrDefault();
//            if (string.IsNullOrEmpty(referrer))
//            {
//                context.Response.StatusCode = StatusCodes.Status403Forbidden;
//                await context.Response.WriteAsync("Hmm, Can't reach this page");
//                return;
//            }
//            else
//            {
//                await next(context);
//            }
//        }
//    }
//}
