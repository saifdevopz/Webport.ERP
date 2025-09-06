namespace Webport.ERP.Identity.Presentation.Endpoints.User;

internal sealed class GetUserByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("user/{Id}", async (
            int Id,
            IQueryHandler<GetUserByIdQuery, GetUserByIdQueryResult> handler,
            CancellationToken cancellationToken) =>
        {
            var response = await handler
                .Handle(new GetUserByIdQuery(Id), cancellationToken)
                .MapResult();

            return response;
        })
        .WithTags(Tags.User);
    }
}
