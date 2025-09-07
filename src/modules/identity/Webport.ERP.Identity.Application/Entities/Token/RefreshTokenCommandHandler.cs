//using Common.Application.CQRS;
//using Common.Domain.Results;
//using System.Application.Dtos;
//using System.Application.Interfaces;

//namespace System.Application.Features.Token;

//public class RefreshTokenCommandHandler(ITokenService tokenService) : ICommandHandler<AccessTokenCommand, Result<AccessTokenResult>>
//{
//    public async Task<Result<AccessTokenResult>> Handle(AccessTokenCommand command, CancellationToken? cancellationToken = null)
//    {
//        var result = await tokenService.RefreshToken(command);
//        return result;
//    }
//}
