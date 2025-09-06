using Webport.ERP.Identity.Application.Dtos;

namespace Webport.ERP.Identity.Application.Interfaces;

public interface ITokenService
{
    Task<Result<TokenResponse>> AccessToken(AccessTokenRequest request);
    Task<Result<TokenResponse>> RefreshToken(RefreshTokenRequest request);
}
