namespace Share.Contract.Events;
public record OrderStarted 
{
    
    public Guid OrderId { get; init; }

    public int Price { get; init; }

    public Guid UserId { get; init; }

}
