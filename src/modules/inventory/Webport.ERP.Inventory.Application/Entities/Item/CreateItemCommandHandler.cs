using FluentValidation;
using Webport.ERP.Inventory.Application.Interfaces;

namespace Webport.ERP.Inventory.Application.Entities.Item;

public class CreateItemCommandHandler(IInventoryRepository<ItemM> repository)
    : ICommandHandler<CreateItemCommand>
{
    public async Task<Result> Handle(
        CreateItemCommand command,
        CancellationToken cancellationToken)
    {
        var model = ItemM.Create(command.ItemCode, command.ItemDesc);

        await repository.AddAsync(model);
        await repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record CreateItemCommand(string ItemCode, string ItemDesc) : ICommand;

public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemCommandValidator()
    {
        RuleFor(_ => _.ItemCode).NotEmpty();
        RuleFor(_ => _.ItemDesc).NotEmpty();
    }
}