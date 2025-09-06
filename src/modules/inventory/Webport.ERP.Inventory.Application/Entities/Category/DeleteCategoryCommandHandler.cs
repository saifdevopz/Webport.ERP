
using FluentValidation;
using Webport.ERP.Inventory.Application.Interfaces;

namespace Webport.ERP.Inventory.Application.Entities.Category;

public class DeleteCategoryCommandHandler(IInventoryRepository<CategoryM> repository)
    : ICommandHandler<DeleteCategoryCommand>
{
    public async Task<Result> Handle(
        DeleteCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var model = await repository.FindOneAsync(_ => _.CategoryId == command.CategoryId, cancellationToken);

        if (model is null)
        {
            return Result.Failure(CustomError.NotFound("Not Found", "Record not found."));
        }

        repository.Delete(model);
        await repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed record DeleteCategoryCommand(int CategoryId) : ICommand;

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(_ => _.CategoryId).NotEmpty();
    }
}