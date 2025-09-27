using FluentValidation;
using Webport.ERP.Inventory.Application.Interfaces;

namespace Webport.ERP.Inventory.Application.Features.Item;

public class DeleteItemCommandHandler(IInventoryRepository<ItemM> repository)
    : ICommandHandler<DeleteItemCommand>
{
    public async Task<Result> Handle(
        DeleteItemCommand command,
        CancellationToken cancellationToken)
    {
        var model = await repository.FindOneAsync(_ => _.ItemId == command.ItemId, cancellationToken);

        if (model is null)
        {
            return Result.Failure(CustomError.NotFound("Not Found", "Record not found."));
        }

        repository.Delete(model);
        await repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record DeleteItemCommand(int ItemId) : ICommand;

public class DeleteItemCommandValidator : AbstractValidator<DeleteItemCommand>
{
    public DeleteItemCommandValidator()
    {
        RuleFor(_ => _.ItemId).NotEmpty();
    }
}