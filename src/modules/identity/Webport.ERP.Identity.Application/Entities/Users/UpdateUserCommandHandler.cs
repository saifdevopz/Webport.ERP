using FluentValidation;
using Webport.ERP.Identity.Application.Interfaces;

namespace Webport.ERP.Identity.Application.Entities.Users;

public class UpdateUserCommandHandler(IIdentityRepository<UserM> repository)
    : ICommandHandler<UpdateUserCommand>
{
    public async Task<Result> Handle(
        UpdateUserCommand command,
        CancellationToken cancellationToken)
    {
        var model = await repository.FindOneAsync(_ => _.UserId == command.UserId, cancellationToken);

        if (model == null)
        {
            return Result.Failure(CustomError.NotFound("Not Found", "User not found"));
        }

        model.FullName = command.FullName;

        repository.Update(model);
        await repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record UpdateUserCommand(
    int UserId,
    string FullName) : ICommand;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(_ => _.UserId).NotEmpty();
        RuleFor(_ => _.FullName).NotEmpty();
    }
}
