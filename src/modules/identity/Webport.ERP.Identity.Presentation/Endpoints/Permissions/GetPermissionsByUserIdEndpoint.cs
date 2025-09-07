namespace Webport.ERP.Identity.Presentation.Endpoints.Permissions;

internal sealed class GetPermissionsByUserIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("permission/{Id}", async (
            int Id,
            IQueryHandler<GetPermissionsByUserIdQuery, GetPermissionsByUserIdQueryResult> handler,
            CancellationToken cancellationToken) =>
        {
            var response = await handler
                .Handle(new GetPermissionsByUserIdQuery(Id), cancellationToken)
                .MapResult();

            return response;
        })
        .WithTags(Tags.Permissions);
    }
}