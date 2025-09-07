using FluentValidation;
using Webport.ERP.Identity.Application.Interfaces;

namespace Webport.ERP.Identity.Application.Entities.Tenant;

public class UpdateTenantCommandHandler(IIdentityRepository<TenantM> repository)
    : ICommandHandler<UpdateTenantCommand>
{
    public async Task<Result> Handle(
        UpdateTenantCommand command,
        CancellationToken cancellationToken)
    {
        var model = await repository.FindOneAsync(_ => _.TenantId == command.TenantId, cancellationToken);

        if (model == null)
        {
            return Result.Failure(CustomError.NotFound("Not Found", "Tenant not found"));
        }

        model.TenantName = command.TenantName;

        repository.Update(model);
        await repository.SaveChangesAsync(cancellationToken);

        return Result.Success(model);
    }
}

public sealed record UpdateTenantCommand(
    int TenantId,
    string TenantName) : ICommand;

public class UpdateTenantCommandValidator : AbstractValidator<UpdateTenantCommand>
{
    public UpdateTenantCommandValidator()
    {
        RuleFor(_ => _.TenantId).NotEmpty();
        RuleFor(_ => _.TenantName).NotEmpty();
    }
}