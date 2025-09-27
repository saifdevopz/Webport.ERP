using Microsoft.AspNetCore.Routing;

namespace Webport.ERP.Common.Presentation.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}