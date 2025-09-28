namespace Webport.ERP.Identity.Presentation.Endpoints.Role;

internal sealed class DeleteRoleEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("role/{Id}", async (
            int Id,
            ICommandHandler<DeleteRoleCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var response = await handler
                .Handle(new DeleteRoleCommand(Id), cancellationToken)
                .MapResult();

            return response;
        })
        .WithTags(Tags.Role);
    }
}