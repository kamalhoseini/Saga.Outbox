using MassTransit;
using Share.Contract.Events;

namespace OrderService.Infrastructure.Saga;

public class OrderStateMachine : MassTransitStateMachine<OrderState>
{
    public OrderStateMachine()
    {
        InstanceState(c => c.CurrentState);

        ConfigureCorrelationIds();

        Initially(
            When(OrderStartedEvent)
            .Then(x =>
            {
                x.Saga.OrderId = x.Message.OrderId;
                x.Saga.DateTime = DateTime.UtcNow;
                x.Saga.Price = x.Message.Price;
                x.Saga.UserId = x.Message.UserId;
            })
            .TransitionTo(Submitted)
            .Publish(context => new OrderSubmitted(context.Saga.CorrelationId)
            {
                OrderId = context.Saga.OrderId,
                Price = context.Saga.Price,
                UserId = context.Saga.UserId
            }));

        During(Submitted,
             When(OrderAcceptedEvent)
               .Then(x => x.Saga.DateTime = DateTime.UtcNow)
               .TransitionTo(Completed),
             When(OrderRejectedEvent)
               .Then(x => x.Saga.DateTime = DateTime.UtcNow)
               .TransitionTo(Rejected)
         );

    }
    private void ConfigureCorrelationIds()
    {
        Event(() => OrderStartedEvent, x => x.CorrelateById(x => x.Message.OrderId));
        Event(() => OrderAcceptedEvent, x => x.CorrelateById(x => x.Message.CorrelationId));
        Event(() => OrderRejectedEvent, x => x.CorrelateById(x => x.Message.CorrelationId));
    }

    public State Started { get; private set; } = default!;
    public State Submitted { get; private set; } = default!;
    public State Accepted { get; private set; } = default!;
    public State Completed { get; private set; } = default!;
    public State Rejected { get; private set; } = default!;
    public Event<OrderStarted> OrderStartedEvent { get; set; } = default!;
    public Event<OrderSubmitted> OrderSubmittedEvent { get; set; } = default!;
    public Event<OrderAccepted> OrderAcceptedEvent { get; set; } = default!;
    public Event<OrderRejected> OrderRejectedEvent { get; set; } = default!;

}