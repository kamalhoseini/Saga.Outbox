namespace Share.Contract.Events;
public record OrderAccepted
{
    public OrderAccepted(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public Guid CorrelationId { get; init; }

    public Guid OrderId { get; init; }
}
