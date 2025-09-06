using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Webport.ERP.Common.Domain.Errors;
using Webport.ERP.Common.Domain.Results;
using Webport.ERP.Common.Infrastructure.Authentication;
using Webport.ERP.Common.Infrastructure.Clock;
using Webport.ERP.Identity.Application.Dtos;
using Webport.ERP.Identity.Application.Interfaces;
using Webport.ERP.Identity.Infrastructure.Database;

namespace Webport.ERP.Identity.Infrastructure.Services;

public class TokenService(
    IOptions<JwtOptions> JwtOptions,
    IIdentityRepository<UserM> repository,
    IdentityDbContext pDbContext,
    IDateTimeProvider dateTimeProvider) : ITokenService
{
    public async Task<Result<TokenResponse>> AccessToken(AccessTokenRequest request)
    {
        UserM? userObj = await repository.FindOneAsync(p => p.Email == request.Email);

        if (userObj is null)
        {
            return Result.Failure<TokenResponse>(CustomError.NotFound("TokenService", "User not found."));
        }
        else
        {
            if (!IdentityMethodExtensions.VerifyPasswordHash(request.Password!, userObj.PasswordHash, userObj.PasswordSalt))
            {
                return Result.Failure<TokenResponse>(CustomError.NotFound("TokenService", "Invalid Credentials."));
            }
        }

        return await GenerateTokensAndUpdateUser(userObj);
    }

    public async Task<Result<TokenResponse>> RefreshToken(RefreshTokenRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        ClaimsPrincipal userPrincipal = GetPrincipalFromExpiredToken(request.Token);
        UserM? user = await repository.FindOneAsync(p => p.Email == userPrincipal.GetUserEmail());

        if (user is null)
        {
            return Result.Failure<TokenResponse>(CustomError.NotFound("404", "User not found."));
        }

        TenantM? tenant = await pDbContext.Tenants.FirstOrDefaultAsync(t => t.TenantId == 1);
        ArgumentNullException.ThrowIfNull(tenant);

        return user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiration <= DateTime.UtcNow
            ? Result.Failure<TokenResponse>(CustomError.NotFound("404", "Invalid Refresh Token."))
            : await GenerateTokensAndUpdateUser(user);
    }

    private async Task<Result<TokenResponse>> GenerateTokensAndUpdateUser(UserM user)
    {
        UserTokenClaims tokenClaims = GetAllUserDetails(user.Email);
        string token = GenerateJwt(tokenClaims);

        user.RefreshToken = IdentityMethodExtensions.GenerateRefreshToken();
        user.RefreshTokenExpiration = DateTime.UtcNow.AddDays(JwtOptions.Value.RefreshTokenExpirationInDays);

        repository.Update(user);
        await repository.SaveChangesAsync();

        var response = new TokenResponse(token, user.RefreshToken, user.RefreshTokenExpiration);

        return Result.Success(response);
    }

    private string GenerateJwt(UserTokenClaims customClaims)
    {
        return GenerateEncryptedToken(GetSigningCredentials(), GetClaims(customClaims));
    }

    private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        JwtSecurityToken token = new(
             claims: claims,
             expires: dateTimeProvider.Now.AddMinutes(JwtOptions.Value.TokenExpirationInMinutes),
             signingCredentials: signingCredentials,
             issuer: JwtOptions.Value.Issuer,
             audience: JwtOptions.Value.Audience);

        JwtSecurityTokenHandler tokenHandler = new();
        return tokenHandler.WriteToken(token);
    }

    private SigningCredentials GetSigningCredentials()
    {
        byte[] secret = Encoding.UTF8.GetBytes(JwtOptions.Value.Key);
        return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
    }

    private static List<Claim> GetClaims(UserTokenClaims customClaims)
    {
        List<Claim> claims =
        [
            new Claim(ClaimTypes.Email, customClaims.Email),
            new Claim(ClaimTypes.Role, customClaims.Role),
            new Claim("TenantName", customClaims.TenantName),
            new Claim(CustomClaims.TenantId, customClaims.TenantId.ToString(CultureInfo.CurrentCulture))
        ];

        return claims;
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        TokenValidationParameters tokenValidationParameters = new()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.Value.Key)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = JwtOptions.Value.Audience,
            ValidIssuer = JwtOptions.Value.Issuer,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero,
        };

        JwtSecurityTokenHandler tokenHandler = new();
        ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken? securityToken);

        return securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase)
            ? throw new SecurityTokenValidationException("Invalid token.")
            : principal;
    }

    private UserTokenClaims GetAllUserDetails(string email)
    {
        var userDetails = pDbContext.Users
            .Where(u => u.Email == email)
            .Include(_ => _.Tenant)
            .Include(_ => _.Role)
            .Select(_ => new UserTokenClaims
            {
                UserId = _.UserId,
                Email = _.Email,
                TenantId = _.TenantId,
                TenantName = _.Tenant!.TenantName,
                Role = _.Role!.RoleName
            })
            .FirstOrDefault();

        return userDetails!;
    }

    private static class IdentityMethodExtensions
    {
        public static string GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[32];
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using HMACSHA512 hmac = new(passwordSalt);
            byte[] computedHash =
                    hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return computedHash.SequenceEqual(passwordHash);
        }
    }
}


