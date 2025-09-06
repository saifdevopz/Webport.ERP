namespace Webport.ERP.Common.Domain.Abstractions;

public interface IDomainEvent
{
    Guid Id { get; }
    DateTime OccurredOnUtc { get; }
}
