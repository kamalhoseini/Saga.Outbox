namespace OrderService.Domain.Entities;

public class Order
{
    public Order(Guid id, Guid userId, int price, DateTime orderDate)
    {
        Id = id;
        UserId = userId;
        Price = price;
        OrderDate = orderDate;
    }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public int Price { get; private set; }
    public DateTime OrderDate { get; private set; }

}
