using MediatR;

namespace OrderService.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommand : IRequest<Guid>
{
    public Guid OrderId { get; init; }
    public Guid UserId { get; init; }
    public int Price { get; init; }
}
