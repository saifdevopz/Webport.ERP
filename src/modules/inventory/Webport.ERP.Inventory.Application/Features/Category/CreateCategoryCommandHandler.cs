using FluentValidation;
using Webport.ERP.Inventory.Application.Interfaces;

namespace Webport.ERP.Inventory.Application.Features.Category;

public sealed class CreateCategoryCommandHandler(IInventoryRepository<CategoryM> repository)
    : ICommandHandler<CreateCategoryCommand>
{
    public async Task<Result> Handle(
        CreateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var model = CategoryM.Create(command.CategoryCode, command.CategoryDesc);

        await repository.AddAsync(model);
        await repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record CreateCategoryCommand(string CategoryCode, string CategoryDesc) : ICommand;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(_ => _.CategoryCode).NotEmpty();
        RuleFor(_ => _.CategoryDesc).NotEmpty();
    }
}

