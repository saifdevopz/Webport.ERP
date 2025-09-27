using FluentValidation;
using Webport.ERP.Identity.Application.Interfaces;

namespace Webport.ERP.Identity.Application.Features.Tenant;

public class CreateTenantCommandHandler(IIdentityRepository<TenantM> repository)
    : ICommandHandler<CreateTenantCommand>
{
    public async Task<Result> Handle(
        CreateTenantCommand command,
        CancellationToken cancellationToken)
    {
        var model = TenantM.Create(command.TenantName);

        await repository.AddAsync(model);
        await repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record CreateTenantCommand(
    string TenantName,
    string DatabaseName) : ICommand;

public class CreateTenantCommandValidator : AbstractValidator<CreateTenantCommand>
{
    public CreateTenantCommandValidator()
    {
        RuleFor(_ => _.TenantName).NotEmpty();
        RuleFor(_ => _.DatabaseName).NotEmpty();
    }
}