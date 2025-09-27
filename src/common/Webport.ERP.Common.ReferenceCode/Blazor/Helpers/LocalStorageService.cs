using Blazored.LocalStorage;

namespace Webport.ERP.Common.ReferenceCode.Blazor.Helpers;

public sealed class LocalStorageService(ILocalStorageService localStorageService)
{
    private const string StorageKey = "authentication-token";
    public async Task<string?> GetToken()
    {
        return await localStorageService.GetItemAsStringAsync(StorageKey);
    }

    public async Task SetToken(string item)
    {
        await localStorageService.SetItemAsStringAsync(StorageKey, item);
    }

    public async Task RemoveToken()
    {
        await localStorageService.RemoveItemAsync(StorageKey);
    }
}

//File Upload
//builder.Services.Configure<FormOptions>(x =>
//{
//    x.ValueLengthLimit = int.MaxValue;
//    x.MultipartBodyLengthLimit = int.MaxValue;
//    x.MultipartBoundaryLengthLimit = int.MaxValue;
//    x.MultipartHeadersCountLimit = int.MaxValue;
//    x.MultipartHeadersLengthLimit = int.MaxValue;
//});


//Configures all requests globally
//builder.WebHost.ConfigureKestrel(serverOptions =>
//{
//    serverOptions.Limits.MaxRequestBodySize = long.MaxValue;
//});


//    public static string GetIpAddress(this HttpContext context)
//{
//    string ip = "N/A";
//    if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var ipList))
//    {
//        ip = ipList.FirstOrDefault() ?? "N/A";
//    }
//    else if (context.Connection.RemoteIpAddress != null)
//    {
//        ip = context.Connection.RemoteIpAddress.MapToIPv4().ToString();
//    }

//    return ip;

//}

//    internal static RouteHandlerBuilder MapTokenGenerationEndpoint(this IEndpointRouteBuilder endpoints)
//{
//    return endpoints.MapPost("/", (LoginDTO request,
//        IJWTService service,
//        HttpContext context,
//        CancellationToken cancellationToken) =>
//    {
//        string ip = context.GetIpAddress();
//        return service.LoginUser(request, ip, cancellationToken);
//    })
//    .WithName(nameof(TokenGenerationEndpoint))
//    .WithSummary("Generate JWT.")
//    .WithDescription("Login User.")
//    .Produces<TokenResponse>(200)
//    .AllowAnonymous();
//}