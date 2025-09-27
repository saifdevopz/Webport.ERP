//using Microsoft.JSInterop;

//namespace Blazor.WebportSystem.Common.Helpers;

//public sealed class CookieService(IJSRuntime jsRuntime)
//{
//    public async Task<string> Get(string key)
//    {
//        return await jsRuntime.InvokeAsync<string>("getCookie", key);
//    }

//    public async Task Remove(string key)
//    {
//        await jsRuntime.InvokeAsync<string>("deleteCookie", key);
//    }

//    public async Task Set(string key, string value, int days)
//    {
//        await jsRuntime.InvokeAsync<string>("setCookie", key, value, days);
//    }
//}