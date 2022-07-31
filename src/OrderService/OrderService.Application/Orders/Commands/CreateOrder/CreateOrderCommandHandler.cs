using MassTransit;
using MediatR;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;
using Share.Contract.Events;

namespace OrderService.Application.Orders.Commands.CreateOrder;

internal class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderContext _context;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateOrderCommandHandler(IOrderContext context, IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _publishEndpoint = publishEndpoint;
    }
    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order(
            request.OrderId,
            request.UserId,
            request.Price,
            DateTime.UtcNow
        );
        await _context.Orders.AddAsync(order);


        await _publishEndpoint.Publish(new OrderStarted
        {
            OrderId = request.OrderId,
            UserId = request.UserId,
            Price = request.Price
        });

        await _context.SaveChangesAsync();

        return order.Id;
    }
}
