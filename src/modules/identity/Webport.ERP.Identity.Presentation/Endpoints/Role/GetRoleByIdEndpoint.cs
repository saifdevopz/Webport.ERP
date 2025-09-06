namespace Webport.ERP.Identity.Presentation.Endpoints.Role;

internal sealed class GetRoleByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("role/{Id}", async (
            int Id,
            IQueryHandler<GetRoleByIdQuery, GetRoleByIdQueryResult> handler,
            CancellationToken cancellationToken) =>
        {
            var response = await handler
                .Handle(new GetRoleByIdQuery(Id), cancellationToken)
                .MapResult();

            return response;
        })
        .WithTags(Tags.Role)
        .RequireAuthorization("inventoy");
    }
}
