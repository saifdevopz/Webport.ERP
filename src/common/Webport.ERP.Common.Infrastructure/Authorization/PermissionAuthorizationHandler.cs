using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Webport.ERP.Common.Application.Authorization;
using Webport.ERP.Common.Domain.Results;
using Webport.ERP.Common.Infrastructure.Authentication;
using Webport.ERP.Common.Infrastructure.Exceptions;

namespace Webport.ERP.Common.Infrastructure.Authorization;

internal sealed class PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory) : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task<Task> HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
    {
        HashSet<string> permissions = await GetPermissions(context);

        if (permissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }

    public async Task<HashSet<string>> GetPermissions(AuthorizationHandlerContext context)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        IPermissionService permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();

        Result<PermissionsResponse> result = await permissionService.GetUserPermissionsAsync(context.User.GetUserId());

        return result.IsFailure
            ? throw new CustomException(nameof(IPermissionService.GetUserPermissionsAsync), result.Error)
            : result.Data.Permissions;
    }

}
