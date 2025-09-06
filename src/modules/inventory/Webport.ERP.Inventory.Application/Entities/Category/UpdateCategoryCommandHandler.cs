using FluentValidation;
using Webport.ERP.Inventory.Application.Interfaces;

namespace Webport.ERP.Inventory.Application.Entities.Category;

public class UpdateCategoryCommandHandler(IInventoryRepository<CategoryM> repository)
    : ICommandHandler<UpdateCategoryCommand, UpdateCategoryResult>
{
    public async Task<Result<UpdateCategoryResult>> Handle(
        UpdateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var model = await repository.FindOneAsync(_ => _.CategoryId == command.CategoryId, cancellationToken);

        if (model == null)
        {
            return Result.Failure<UpdateCategoryResult>(CustomError.NotFound("Not Found", "Record not found."));
        }

        model.CategoryDesc = command.CategoryDesc;

        repository.Update(model);
        await repository.SaveChangesAsync(cancellationToken);

        return Result.Success(new UpdateCategoryResult(model));
    }
}

public sealed record UpdateCategoryCommand(int CategoryId, string CategoryDesc) : ICommand<UpdateCategoryResult>;

public sealed record UpdateCategoryResult(CategoryM Result);

public class UpdateRoleCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(_ => _.CategoryId).NotEmpty();
        RuleFor(_ => _.CategoryDesc).NotEmpty();
    }
}