namespace Share.Contract.Events;
public record OrderRejected
{
    public OrderRejected(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public Guid CorrelationId { get; init; }

    public Guid OrderId { get; init; }

    public string? Reason { get; init; }
}
