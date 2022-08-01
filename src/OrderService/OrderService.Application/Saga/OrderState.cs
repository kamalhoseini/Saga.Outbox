using MassTransit;

namespace OrderService.Application.Saga;

public class OrderState : SagaStateMachineInstance
{
    public string CurrentState { get; set; } = null!;
    public Guid CorrelationId { get; set; }
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public int Price { get; set; }
    public DateTime DateTime { get; set; }
}