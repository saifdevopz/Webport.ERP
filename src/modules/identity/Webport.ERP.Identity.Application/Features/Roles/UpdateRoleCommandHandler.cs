using FluentValidation;
using Webport.ERP.Identity.Application.Interfaces;

namespace Webport.ERP.Identity.Application.Features.Roles;

public class UpdateRoleCommandHandler(IIdentityRepository<RoleM> repository)
    : ICommandHandler<UpdateRoleCommand>
{
    public async Task<Result> Handle(
        UpdateRoleCommand command,
        CancellationToken cancellationToken)
    {
        var model = await repository.FindOneAsync(_ => _.RoleId == command.RoleId, cancellationToken);

        if (model == null)
        {
            return Result.Failure(CustomError.NotFound("Not Found", "Role not found."));
        }

        model.RoleName = command.RoleName;
        model.NormalizedRoleName = command.RoleName.ToUpperInvariant();

        repository.Update(model);
        await repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record UpdateRoleCommand(int RoleId, string RoleName) : ICommand;

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(_ => _.RoleId).NotEmpty();
        RuleFor(_ => _.RoleName).NotEmpty();
    }
}