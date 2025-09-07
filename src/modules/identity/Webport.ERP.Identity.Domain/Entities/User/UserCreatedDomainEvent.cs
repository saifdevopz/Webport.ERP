namespace Webport.ERP.Identity.Domain.Entities.User;

public sealed class UserCreatedDomainEvent(int tenantId, int userId) : DomainEvent
{
    public int TenantId { get; init; } = tenantId;
    public int UserId { get; init; } = userId;
}