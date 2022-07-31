using MediatR;
using OrderService.Application.Interfaces;

namespace OrderService.Application.Orders.Commands.DeleteOrder;

internal class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly IOrderContext _context;

    public DeleteOrderCommandHandler(IOrderContext context)
    {
        _context = context;
    }
    public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders.FindAsync(request.OrderId);
        if (order is null)
            throw new KeyNotFoundException($"Order with id {request.OrderId} not found");

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
