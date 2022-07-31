using MassTransit;
using Microsoft.Extensions.Logging;
using Share.Contract.Events;

namespace OrderService.Application.Orders.EventHandlers;

public class OrderAcceptedEventHandler : IConsumer<OrderAccepted>
{
    private readonly ILogger<OrderAcceptedEventHandler> _logger;
    public OrderAcceptedEventHandler(ILogger<OrderAcceptedEventHandler> logger)
    {
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<OrderAccepted> context)
    {
        _logger.LogInformation($"Order {context.Message.OrderId} accepted");
        await Task.CompletedTask;
    }
}
