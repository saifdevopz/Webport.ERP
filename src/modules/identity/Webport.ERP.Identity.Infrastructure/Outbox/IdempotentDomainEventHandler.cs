using Dapper;
using System.Data.Common;
using Webport.ERP.Common.Application.Database;
using Webport.ERP.Common.Application.Messaging;
using Webport.ERP.Common.Domain.Abstractions;
using Webport.ERP.Common.Infrastructure.Outbox;

namespace Webport.ERP.Identity.Infrastructure.Outbox;

internal sealed class IdempotentDomainEventHandler<TDomainEvent>(
        IDomainEventDispatcher<TDomainEvent> decorated,
        IDbConnectionFactory _dbConnectionFactory)
        : DomainEventDispatcher<TDomainEvent>
        where TDomainEvent : IDomainEvent
{
    public override async Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await _dbConnectionFactory.OpenPostgreSQLConnection();

        OutboxMessageConsumer outboxMessageConsumer = new(domainEvent.Id, decorated.GetType().Name);

        if (await OutboxConsumerExistsAsync(connection, outboxMessageConsumer))
        {
            return;
        }

        await decorated.Handle(domainEvent, cancellationToken);

        await InsertOutboxConsumerAsync(connection, outboxMessageConsumer);
    }

    private static async Task<bool> OutboxConsumerExistsAsync(
            DbConnection dbConnection,
            OutboxMessageConsumer outboxMessageConsumer)
    {
        const string sql =
            $"""
            SELECT EXISTS(
                SELECT 1
                FROM {IdentityConstants.Schema}.outbox_message_consumers
                WHERE outbox_message_id = @OutboxMessageId AND
                      name = @Name
            )
            """;

        return await dbConnection.ExecuteScalarAsync<bool>(sql, outboxMessageConsumer);
    }

    private static async Task InsertOutboxConsumerAsync(
            DbConnection dbConnection,
            OutboxMessageConsumer outboxMessageConsumer)
    {
        const string sql =
            $"""
            INSERT INTO {IdentityConstants.Schema}.outbox_message_consumers(outbox_message_id, name)
            VALUES (@OutboxMessageId, @Name)
            """;

        await dbConnection.ExecuteAsync(sql, outboxMessageConsumer);
    }
}
