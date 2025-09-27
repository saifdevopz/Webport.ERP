//using System.Net.Http.Headers;
//using Webport.ERP.Common.ReferenceCode.Blazor.Helpers;

//namespace Clients.BlazorWASM.Helpers
//{
//    public class CustomHttpClient(IHttpClientFactory httpClientFactory, LocalStorageService localStorageService)
//    {
//        private const string HeaderKey = "Authorization";
//        public async Task<HttpClient> GetPrivateHttpClient()
//        {
//            var client = httpClientFactory.CreateClient("SystemApiClient");
//            var stringToken = await localStorageService.GetToken();
//            if (string.IsNullOrEmpty(stringToken)) return client;

//            var deserializeToken = Serialization.DeserializeJsonString<TokenResponse>(stringToken);
//            if (deserializeToken == null) return client;

//            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", deserializeToken.Token);

//            return client;
//        }

//        public HttpClient GetPublicHttpClient()
//        {
//            var client = httpClientFactory.CreateClient("SystemApiClient");
//            client.DefaultRequestHeaders.Remove(HeaderKey);
//            return client;
//        }


//    }
//}




//builder.Services.AddTransient<CustomHttpDelegate>();
//builder.Services.AddHttpClient("SystemApiClient", client =>
//{
//    client.BaseAddress = new Uri("https://localhost:7000/");
//}).AddHttpMessageHandler<CustomHttpDelegate>();

//builder.Services.AddBlazoredLocalStorage();
//builder.Services.AddAuthorizationCore();

//builder.Services.AddScoped<CustomHttpClient>();