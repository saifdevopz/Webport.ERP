//using System.Net;
//using System.Net.Http.Headers;
//using Webport.ERP.Common.ReferenceCode.Blazor.Helpers;

//namespace Clients.BlazorWASM.Helpers
//{
//    public class CustomHttpDelegate(LocalStorageService _localStorageService, IJWTService _jwtService) : DelegatingHandler
//    {
//        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
//        {
//            bool loginUrl = request.RequestUri!.AbsoluteUri.Contains("login");
//            bool registerUrl = request.RequestUri!.AbsoluteUri.Contains("register");
//            bool refreshTokenUrl = request.RequestUri!.AbsoluteUri.Contains("refresh-token");

//            if (loginUrl || registerUrl || refreshTokenUrl) return await base.SendAsync(request, cancellationToken);

//            var result = await base.SendAsync(request, cancellationToken);
//            if (result.StatusCode == HttpStatusCode.Unauthorized)
//            {
//                var stringToken = await _localStorageService.GetToken();
//                if (stringToken == null) return result;

//                string token = string.Empty;
//                try
//                {
//                    token = request.Headers.Authorization!.Parameter!;
//                }
//                catch { }

//                var deserializedToken = Serialization.DeserializeJsonString<TokenResponse>(stringToken);
//                if (deserializedToken is null) return result;

//                if (string.IsNullOrEmpty(token))
//                {
//                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", deserializedToken.Token);
//                    return await base.SendAsync(request, cancellationToken);
//                }

//                // Call for refresh token.
//                var newJwtToken = await GetRefreshToken(deserializedToken, cancellationToken);
//                if (string.IsNullOrEmpty(newJwtToken.Token)) return result;

//                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newJwtToken.Token);
//                return await base.SendAsync(request, cancellationToken);

//            }

//            return result;
//        }

//        private async Task<TokenResponse> GetRefreshToken(TokenResponse tokens, CancellationToken cancellationToken)
//        {
//            var result = await _jwtService.GetTokenWithRefreshToken(new TokenRequest(tokens.Token, tokens.RefreshToken), "N/A", cancellationToken);
//            string serializedToken = Serialization.SerializeObj(new TokenResponse() { Token = result.Token, RefreshToken = result.RefreshToken, RefreshTokenExpiryTime = result.RefreshTokenExpiryTime });
//            await _localStorageService.SetToken(serializedToken);
//            return result;
//        }

//    }
//}
