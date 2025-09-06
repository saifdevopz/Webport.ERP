﻿using Webport.ERP.Common.Domain.Abstractions;

namespace Webport.ERP.Common.Application.Messaging;

public abstract class DomainEventDispatcher<TDomainEvent> : IDomainEventDispatcher<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    public abstract Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken);

    public Task Handle(IDomainEvent domainEvent, CancellationToken cancellationToken) =>
        Handle((TDomainEvent)domainEvent, cancellationToken);
}