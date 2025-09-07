using FluentValidation;
using Webport.ERP.Identity.Application.Interfaces;

namespace Webport.ERP.Identity.Application.Entities.Roles;

public class CreateRoleCommandHandler(IIdentityRepository<RoleM> repository)
    : ICommandHandler<CreateRoleCommand>
{
    public async Task<Result> Handle(
        CreateRoleCommand command,
        CancellationToken cancellationToken)
    {
        var model = RoleM.Create(command.RoleName);

        await repository.AddAsync(model);
        await repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record CreateRoleCommand(string RoleName) : ICommand;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(_ => _.RoleName).NotEmpty();
    }
}