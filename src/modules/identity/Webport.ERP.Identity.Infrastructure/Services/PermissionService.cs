using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webport.ERP.Common.Application.Authorization;
using Webport.ERP.Common.Application.Messaging;
using Webport.ERP.Common.Domain.Results;
using Webport.ERP.Identity.Application.Features.Permissions;

namespace Webport.ERP.Identity.Infrastructure.Services;


internal sealed class PermissionService(
    IQueryHandler<GetPermissionsByUserIdQuery, GetPermissionsByUserIdQueryResult> handler)
    : IPermissionService
{
    public async Task<Result<PermissionsResponse>> GetUserPermissionsAsync(int userId)
    {
        var response = await handler
            .Handle(new GetPermissionsByUserIdQuery(userId), default);

        return Result.Success(response.Data.Permissions);
    }
}