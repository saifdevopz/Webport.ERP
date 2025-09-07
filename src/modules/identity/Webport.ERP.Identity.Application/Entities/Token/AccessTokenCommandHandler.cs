using FluentValidation;
using Webport.ERP.Identity.Application.Dtos;
using Webport.ERP.Identity.Application.Interfaces;

namespace Webport.ERP.Identity.Application.Entities.Token;

public class AccessTokenCommandHandler(ITokenService tokenService)
    : ICommandHandler<AccessTokenCommand, AccessTokenResult>
{
    public async Task<Result<AccessTokenResult>> Handle(
        AccessTokenCommand command,
        CancellationToken cancellationToken)
    {
        Result<TokenResponse> tokenResult = await tokenService.AccessToken(new AccessTokenRequest(command.Email, command.Password));

        if (tokenResult.IsFailure)
        {
            return Result.Failure<AccessTokenResult>(tokenResult.Error!);
        }

        return Result.Success(new AccessTokenResult(
            tokenResult.Data.Token,
            tokenResult.Data.RefreshToken));
    }
}

public sealed record AccessTokenCommand(string Email, string Password) : ICommand<AccessTokenResult>;

public sealed record AccessTokenResult(string Token, string RefreshToken);

public class AccessTokenCommandValidator : AbstractValidator<AccessTokenCommand>
{
    public AccessTokenCommandValidator()
    {
        RuleFor(_ => _.Email).NotEmpty();
        RuleFor(_ => _.Password).NotEmpty();
    }
}