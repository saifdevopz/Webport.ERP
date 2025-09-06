﻿namespace Webport.ERP.Identity.Presentation.Endpoints.Tenant;

internal sealed class GetTenantsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("tenant", async (
            IQueryHandler<GetTenantsQuery, GetTenantsQueryResult> handler,
            CancellationToken cancellationToken) =>
        {
            var response = await handler
                .Handle(new GetTenantsQuery(), cancellationToken)
                .MapResult();

            return response;
        })
        .WithTags(Tags.Tenant);
    }
}


