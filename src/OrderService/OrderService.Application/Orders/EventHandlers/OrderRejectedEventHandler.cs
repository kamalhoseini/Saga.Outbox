using MassTransit;
using MediatR;
using OrderService.Application.Orders.Commands.DeleteOrder;
using Share.Contract.Events;

namespace OrderService.Application.Orders.EventHandlers;

public class OrderRejectedEventHandler : IConsumer<OrderRejected>
{
    private readonly IMediator _mediator;
    public OrderRejectedEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task Consume(ConsumeContext<OrderRejected> context)
    {
        await _mediator.Send(new DeleteOrderCommand(context.Message.OrderId));
    }
}
