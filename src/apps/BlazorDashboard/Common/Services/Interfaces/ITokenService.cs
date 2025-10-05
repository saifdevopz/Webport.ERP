using Webport.ERP.Common.Domain.Contracts.Identity;
using Webport.ERP.Common.Domain.Results;

namespace BlazorDashboard.Common.Services.Interfaces;

public interface ITokenService
{
    Task<Result<TokenResponse>> AccessToken(LoginDto request, CancellationToken cancellationToken = default);
    Task<TokenResponse> RefreshToken(RefreshTokenRequest request, CancellationToken cancellationToken = default);
}