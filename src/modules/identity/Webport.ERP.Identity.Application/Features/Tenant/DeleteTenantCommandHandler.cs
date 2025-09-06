using Webport.ERP.Identity.Application.Interfaces;

namespace Webport.ERP.Identity.Application.Features.Tenant;

public class DeleteTenantCommandHandler(IIdentityRepository<TenantM> repository)
    : ICommandHandler<DeleteTenantCommand>
{
    public async Task<Result> Handle(
        DeleteTenantCommand command,
        CancellationToken cancellationToken)
    {
        var model = await repository.FindOneAsync(_ => _.TenantId == command.TenantId, cancellationToken);

        if (model is null)
        {
            return Result.Failure(CustomError.NotFound("Not Found", "Tenant not found."));
        }

        repository.Delete(model);
        await repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record DeleteTenantCommand(int TenantId) : ICommand;