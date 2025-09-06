using Webport.ERP.Identity.Application.Interfaces;

namespace Webport.ERP.Identity.Application.Features.Users;

public class DeleteUserCommandHandler(IIdentityRepository<UserM> repository)
    : ICommandHandler<DeleteUserCommand>
{
    public async Task<Result> Handle(
        DeleteUserCommand command,
        CancellationToken cancellationToken)
    {
        var model = await repository.FindOneAsync(_ => _.UserId == command.UserId, cancellationToken);

        if (model is null)
        {
            return Result.Failure(CustomError.NotFound("Not Found", "User not found."));
        }

        repository.Delete(model);
        await repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record DeleteUserCommand(int UserId) : ICommand;