namespace Webport.ERP.Identity.Domain.Entities.User;

public sealed class UserCreatedDomainEvent(int userId) : DomainEvent
{
    public int UserId { get; init; } = userId;
}