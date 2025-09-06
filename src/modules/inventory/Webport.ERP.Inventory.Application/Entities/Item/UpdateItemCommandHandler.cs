using FluentValidation;
using Webport.ERP.Inventory.Application.Interfaces;

namespace Webport.ERP.Inventory.Application.Entities.Item;

public class UpdateItemCommandHandler(IInventoryRepository<ItemM> repository)
    : ICommandHandler<UpdateItemCommand>
{
    public async Task<Result> Handle(
        UpdateItemCommand command,
        CancellationToken cancellationToken)
    {
        var model = await repository.FindOneAsync(_ => _.ItemId == command.ItemId, cancellationToken);

        if (model == null)
        {
            return Result.Failure(CustomError.NotFound("Not Found", "Record not found."));
        }

        model.ItemDesc = command.ItemDesc;

        repository.Update(model);
        await repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record UpdateItemCommand(int ItemId, string ItemDesc) : ICommand;

public class UpdateItemCommandValidator : AbstractValidator<UpdateItemCommand>
{
    public UpdateItemCommandValidator()
    {
        RuleFor(_ => _.ItemId).NotEmpty();
        RuleFor(_ => _.ItemDesc).NotEmpty();
    }
}