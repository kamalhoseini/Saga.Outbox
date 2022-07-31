namespace Share.Contract.Events;
public record OrderSubmitted 
{
    public OrderSubmitted(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public Guid CorrelationId { get; init; }
    public Guid OrderId { get; init; }
    
    public int Price { get; init; }

    public Guid UserId { get; init; }
}
