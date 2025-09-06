using Microsoft.AspNetCore.Http;
using Webport.ERP.Common.Infrastructure.Authentication;

namespace Webport.ERP.Common.Infrastructure.Database;

public sealed class TenantProvider(IHttpContextAccessor httpContextAccessor)
{
    public int TenantId
    {
        get
        {
            var user = httpContextAccessor.HttpContext?.User;

            return user.GetTenantId();
        }
    }

    public string UserEmail
    {
        get
        {
            var user = httpContextAccessor.HttpContext?.User;

            return user.GetUserEmail();
        }
    }
}

