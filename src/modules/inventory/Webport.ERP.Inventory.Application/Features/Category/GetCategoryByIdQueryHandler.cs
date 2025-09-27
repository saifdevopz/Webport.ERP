using Webport.ERP.Inventory.Application.Interfaces;

namespace Webport.ERP.Inventory.Application.Features.Category;

public class GetCategoryByIdQueryHandler(IInventoryRepository<CategoryM> repository)
    : IQueryHandler<GetCategoryByIdQuery, GetCategoryByIdQueryResult>
{
    public async Task<Result<GetCategoryByIdQueryResult>> Handle(
        GetCategoryByIdQuery query,
        CancellationToken cancellationToken)
    {
        var model = await repository.FindOneAsync(_ => _.CategoryId == query.CategoryId, cancellationToken);

        return model is not null
            ? Result.Success(new GetCategoryByIdQueryResult(model))
            : Result.Failure<GetCategoryByIdQueryResult>(CustomError.NotFound("Not Found", "Record not found."));
    }
}

public sealed record GetCategoryByIdQuery(int CategoryId) : IQuery<GetCategoryByIdQuery>;

public sealed record GetCategoryByIdQueryResult(CategoryM Category);
