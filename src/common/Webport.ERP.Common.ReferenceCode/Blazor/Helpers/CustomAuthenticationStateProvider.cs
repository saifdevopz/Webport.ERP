//using Microsoft.AspNetCore.Components.Authorization;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using Webport.ERP.Common.ReferenceCode.Blazor.Helpers;

//namespace Clients.BlazorWASM.Helpers
//{
//    public class CustomAuthenticationStateProvider(LocalStorageService _localStorageService) : AuthenticationStateProvider
//    {
//        private readonly ClaimsPrincipal anonymous = new(new ClaimsIdentity());
//        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
//        {
//            var stringToken = await _localStorageService.GetToken();
//            if (string.IsNullOrWhiteSpace(stringToken)) return await Task.FromResult(new AuthenticationState(anonymous));

//            var deserializeToken = Serialization.DeserializeJsonString<TokenResponse>(stringToken);
//            if (deserializeToken == null) return await Task.FromResult(new AuthenticationState(anonymous));

//            var getUserClaims = GetClaimsFromToken(deserializeToken.Token!);
//            if (getUserClaims == null) return await Task.FromResult(new AuthenticationState(anonymous));

//            // Checks the exp field of the token
//            var expiry = getUserClaims.exp;
//            if (expiry == null) return await Task.FromResult(new AuthenticationState(anonymous));

//            // The exp field is in Unix time
//            var datetime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expiry));
//            if (datetime.UtcDateTime <= DateTime.UtcNow) return await Task.FromResult(new AuthenticationState(anonymous));

//            var claimsPrincipal = SetClaimPrincipal(getUserClaims);
//            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
//        }

//        public async Task UpdateAuthenticationState(TokenResponse session)
//        {
//            ClaimsPrincipal claimsPrincipal;

//            if (!string.IsNullOrEmpty(session.Token))
//            {
//                var serializeSession = Serialization.SerializeObj(session);
//                await _localStorageService.SetToken(serializeSession);
//                var getUserClaims = GetClaimsFromToken(session.Token!);
//                claimsPrincipal = SetClaimPrincipal(getUserClaims);
//            }
//            else
//            {
//                claimsPrincipal = anonymous;
//                await _localStorageService.RemoveToken();
//            }

//            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
//        }

//        public static ClaimsPrincipal SetClaimPrincipal(CustomUserClaim model)
//        {
//            return new ClaimsPrincipal
//            (
//                new ClaimsIdentity
//                (
//                    new List<Claim>
//                    {
//                        new(ClaimTypes.NameIdentifier, model.Id),
//                        new(ClaimTypes.Name, model.Username),
//                        new(ClaimTypes.Email, model.Email),
//                        new(ClaimTypes.Role, model.Role),
//                        new("tenant", model.Tenant),
//                    }, "JwtAuth"
//                )
//            );
//        }

//        public static CustomUserClaim GetClaimsFromToken(string jwtToken)
//        {
//            if (string.IsNullOrEmpty(jwtToken)) return new CustomUserClaim();

//            var handler = new JwtSecurityTokenHandler();
//            var token = handler.ReadJwtToken(jwtToken);
//            var claims = token.Claims;

//            string Id = claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value!;
//            string Name = claims.First(c => c.Type == ClaimTypes.Name).Value!;
//            string Email = claims.First(c => c.Type == ClaimTypes.Email).Value!;
//            string Role = claims.First(c => c.Type == ClaimTypes.Role).Value!;
//            string Tenant = claims.First(c => c.Type == "tenant").Value!;
//            string exp = claims.First(claim => claim.Type.Equals("exp")).Value;

//            return new CustomUserClaim(Id!, Name!, Email!, Role!, Tenant!, exp);
//        }

//    }
//}
