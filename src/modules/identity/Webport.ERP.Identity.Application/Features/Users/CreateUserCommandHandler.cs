using FluentValidation;
using Webport.ERP.Identity.Application.Interfaces;

namespace Webport.ERP.Identity.Application.Features.Users;

public class CreateUserCommandHandler(IIdentityRepository<UserM> repository)
    : ICommandHandler<CreateUserCommand>
{
    public async Task<Result> Handle(
        CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        var model = UserM.Create(command.TenantId, command.RoleId, command.FullName, command.Email, command.Password);

        await repository.AddAsync(model);
        await repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record CreateUserCommand(
    string FullName,
    string Email,
    string Password,
    int TenantId,
    int RoleId) : ICommand;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(_ => _.FullName).NotEmpty();
        RuleFor(_ => _.Email).NotEmpty().EmailAddress();
        RuleFor(_ => _.Password).NotEmpty();
        RuleFor(_ => _.TenantId).NotEmpty();
    }
}