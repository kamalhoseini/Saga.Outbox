//using MassTransit;
//using Share.Contract.Events;

//namespace OrderService.Application.Orders.EventHandlers;

//public class OrderSubmittedEventHandler : IConsumer<OrderSubmitted>
//{
//    private readonly IPublishEndpoint _publishEndpoint;

//    public OrderSubmittedEventHandler(IPublishEndpoint publishEndpoint)
//    {
//        _publishEndpoint = publishEndpoint;
//    }
//    public async Task Consume(ConsumeContext<OrderSubmitted> context)
//    {
//        if (context.Message.Price < 1000)
//        {
//            await _publishEndpoint.Publish(new OrderAccepted(context.Message.CorrelationId)
//            {
//                OrderId = context.Message.OrderId
//            });
//        }
//        else
//        {

//            await _publishEndpoint.Publish(new OrderRejected(context.Message.CorrelationId)
//            {
//                OrderId = context.Message.OrderId,
//                Reason = "You don't have enough credit"

//            });
//        }
//    }
//}
