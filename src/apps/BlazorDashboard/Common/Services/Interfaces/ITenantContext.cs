﻿using System.Security.Claims;

namespace BlazorDashboard.Common.Services.Interfaces;

public interface ITenantContext
{
    string? TenantId { get; }
    string? TenantName { get; }
    string? UserId { get; }
    string? Role { get; }
    string? AccessToken { get; }
    ClaimsPrincipal? User { get; }
    void InitializeFromUser(ClaimsPrincipal user);
}